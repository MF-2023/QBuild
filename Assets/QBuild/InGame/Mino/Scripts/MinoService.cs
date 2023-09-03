using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using QBuild.Stage;
using UniRx;
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

        public IObservable<Polyomino> MinoContacted => _onMinoContactedSubject;
        private Subject<Polyomino> _onMinoContactedSubject = new();

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

        private bool MinoMove(Polyomino mino, Vector3Int move)
        {
            var blocks = mino.GetBlocks();

            foreach (var block in blocks)
            {
                if (block.CanMove(move)) continue;

                _blockService.TryGetBlock(block.GetGridPosition() + move, out var otherBlock);
                if (block.GetMinoKey() == otherBlock.GetMinoKey()) continue;

                return false;
            }

            foreach (var block in blocks)
            {
                block.MoveNext(move);
            }

            OnMinoMoved?.Invoke(mino);
            return true;
        }

        public async UniTask TranslateFallMino(Polyomino mino, Vector3Int move)
        {
            await TranslateMino(mino, move);
        }

        public async UniTask TranslateMino(Polyomino mino, Vector3Int move)
        {
            mino.StartTranslate();
            
            if (MinoMove(mino, move))
            {
                cancellation?.Cancel();
                cancellation = null;
            }

            await AsyncMinoContact(mino);
            
            mino.FinishTranslate();
        }

        private async UniTask AsyncMinoContact(Polyomino mino)
        {
            if (cancellation != null) return;
            var blocks = mino.GetBlocks();
            
            var dirs = new Vector3Int[]
            {
                new(0, -1, 0)
            };
            
            foreach (var block in blocks)
            {
                foreach (var pos in dirs.Select(x => x + block.GetGridPosition()))
                {
                    if (!_blockService.TryGetBlock(pos, out var dirBlock)) continue;
                    if (dirBlock.IsFalling()) continue;
                    if (!_blockService.ContactCondition(block, dirBlock)) continue;
                    cancellation = new CancellationTokenSource();
                    Debug.Log("MinoService Contact");
                    if (block.GetGridPosition().y >= 9)
                    {
                        Debug.LogError($"ミノの位置が異常です:{dirBlock.GetGridPosition()}");
                    }

                    await AsyncOnMinoContacted(mino, cancellation.Token);
                    cancellation?.Cancel();
                    cancellation = null;
                    break;
                }

                if (!mino.IsFalling) break;
            }
        }

        
        public void MinoContact(Polyomino mino)
        {
            var blocks = mino.GetBlocks();
            
            var dirs = new Vector3Int[]
            {
                new(1, 0, 0),
                new(-1, 0, 0),
                new(0, 0, 1),
                new(0, 0, -1),
                new(0, -1, 0)
            };
            
            foreach (var block in blocks)
            {
                foreach (var pos in dirs.Select(x => x + block.GetGridPosition()))
                {
                    if (!_blockService.TryGetBlock(pos, out var dirBlock)) continue;
                    if (dirBlock.IsFalling()) continue;
                    if (!_blockService.ContactCondition(block, dirBlock)) continue;
                    Place(mino);
                    cancellation?.Cancel();
                    cancellation = null;
                    break;
                }

                if (!mino.IsFalling) break;
            }
        }
        private CancellationTokenSource cancellation;

        private async UniTask AsyncOnMinoContacted(Polyomino mino, CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("Canceled.");
                    return;
                }

                Place(mino);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log("AsyncOnMinoContacted Cancel");
            }
        }
        public void Place(Polyomino mino)
        {
            Debug.Log($"MinoService.Place {mino.GetBlocks()[0].name}");
            if (!mino.IsFalling) return;
            if (JointMino(mino))
            {
                OnMinoPlaced?.Invoke(mino);
                return;
            }

            mino.Place();
            OnMinoPlaced?.Invoke(mino);
        }

        public bool JointMino(Polyomino mino)
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
                    otherMino.JointMino(mino);
                    return true;
                }
            }

            return false;
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