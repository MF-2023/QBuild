using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Stage;
using UnityEngine;
using VContainer;

namespace QBuild.Mino
{
    /// <summary>
    /// ミノの振る舞いを定義するクラス
    /// </summary>
    public class MinoService
    {
        public event Action<Polyomino> OnMinoPlaced;
        public event Action<Polyomino> OnMinoMoved; 

        [Inject]
        public MinoService(MinoStore minoStore, BlockService blockService, StabilityCalculator stabilityCalculator,
            IMinoFactory minoFactory, MinoTypeList minoTypeList, StageScriptableObject stageScriptableObject
        )
        {
            _minoStore = minoStore;
            _blockService = blockService;
            _stabilityCalculator = stabilityCalculator;
            _minoFactory = minoFactory;
            _minoTypeList = minoTypeList;
            _stageScriptableObject = stageScriptableObject;
        }

        public MinoKey SpawnMino()
        {
            var minoType = _minoTypeList.NextGenerator();
            var mino = _minoFactory.CreateMino(minoType, _stageScriptableObject.MinoSpawnPosition, null);
            return mino.GetStoreKey();
        }

        public bool TryGetMino(MinoKey key, out Polyomino polyomino)
        {
            return _minoStore.TryGetMino(key, out polyomino);
        }

        public void TranslationMino(Polyomino mino, Vector3Int move)
        {
            var dirs = new Vector3Int[]
            {
                new(1, 0, 0),
                new(-1, 0, 0),
                new(0, 0, 1),
                new(0, 0, -1),
                new(0, -1, 0)
            };
            var blocks = mino.GetBlocks();
            var shouldMove = true;


            foreach (var block in blocks)
            {
                if (block.CanMove(move)) continue;

                _blockService.TryGetBlock(block.GetGridPosition() + move, out var otherBlock);
                if (block.GetMinoKey() == otherBlock.GetMinoKey()) continue;

                shouldMove = false;
            }

            if (shouldMove)
            {
                foreach (var block in blocks)
                {
                    block.MoveNext(move);
                }
                OnMinoMoved?.Invoke(mino);
            }

            foreach (var block in blocks)
            {
                foreach (var pos in dirs.Select(x => x + block.GetGridPosition()))
                {
                    if (!_blockService.TryGetBlock(pos, out var dirBlock)) continue;
                    if (dirBlock.IsFalling()) continue;
                    if (!_blockService.ContactCondition(block, dirBlock)) continue;

                    Place(mino);
                    break;
                }

                if (!mino.IsFalling) break;
            }
        }

        public void Place(Polyomino mino)
        {
            if (ContactMino(mino))
            {
                return;
            }

            mino.Place();
            OnMinoPlaced?.Invoke(mino);
        }

        public bool ContactMino(Polyomino mino)
        {
            foreach (var block in mino.GetBlocks())
            {
                foreach (var dir in Vector3IntDirs.AllDirections)
                {
                    var targetPos = block.GetGridPosition() + dir;
                    if (targetPos.y <= 0) continue;
                    if (!_blockService.TryGetBlock(targetPos, out var targetBlock)) continue;
                    if (targetBlock.IsFalling()) continue;
                    if (targetBlock.GetMinoKey() == block.GetMinoKey()) continue;
                    if (!TryGetMino(targetBlock.GetMinoKey(), out var otherMino)) continue;
                    if (!BlockService.CanJoint(block, targetBlock)) continue;
                    otherMino.ContactMino(mino);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ミノの落下予測位置を計算する
        /// </summary>
        /// <param name="mino">ミノ</param>
        /// <returns>各ブロックの落下先</returns>
        public IEnumerable<Vector3Int> GetProvisionalPlacePosition(Polyomino mino)
        {
            var dirs = new Vector3Int[]
            {
                new(1, 0, 0),
                new(-1, 0, 0),
                new(0, 0, 1),
                new(0, 0, -1),
                new(0, -1, 0)
            };

            var checkRowMin = int.MinValue;

            var blocks = mino.GetBlocks();
            foreach (var block in blocks)
            {
                var empty = false;
                var checkRow = 0;
                do
                {
                    var blockPos = block.GetGridPosition() + new Vector3Int(0, checkRow, 0);
                    foreach (var pos in dirs.Select(x => x + blockPos))
                    {
                        if (!_blockService.TryGetBlock(pos, out var dirBlock)) continue;
                        if (dirBlock.IsFalling()) continue;
                        if (checkRowMin < checkRow) checkRowMin = checkRow;
                        empty = true;
                        break;
                    }

                    checkRow--;
                } while (!empty);
            }

            return blocks.Select(block => block.GetGridPosition() + new Vector3Int(0, checkRowMin, 0)).ToList();
        }

        public bool DestroyMino(Polyomino mino)
        {
            foreach (var block in mino.GetBlocks())
            {
                _blockService.RemoveBlock(block);
            }

            return _minoStore.RemoveMino(mino.GetStoreKey());
        }

        public void Clear()
        {
            _minoStore.Clear();
        }

        private readonly StabilityCalculator _stabilityCalculator;

        private readonly BlockService _blockService;
        private readonly MinoStore _minoStore;

        private readonly IMinoFactory _minoFactory;
        private readonly MinoTypeList _minoTypeList;
        private readonly StageScriptableObject _stageScriptableObject;
    }
}