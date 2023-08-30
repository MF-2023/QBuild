using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace QBuild.Mino.ProvisionalMino
{
    /// <summary>
    /// 落下予定地点表示に関する振る舞いを定義するクラス
    /// </summary>
    public class ProvisionalMinoService
    {
        [Inject]
        public ProvisionalMinoService(BlockService blockService)
        {
            _blockService = blockService;
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
        
        private readonly BlockService _blockService;
    }
}