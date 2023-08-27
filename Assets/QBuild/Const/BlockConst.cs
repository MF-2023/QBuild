using UnityEngine;

namespace QBuild.Const
{
    public static class BlockConst
    {
        /// <summary>
        /// Blockの一辺当たりの長さを決める定数
        /// </summary>
        public const float BlockScale = 1f;

        /// <summary>
        /// Blockの長さ半分を表す定数
        /// </summary>
        public const float BlockScaleHalf = BlockScale / 2f;

        /// <summary>
        /// Blockの大きさを表す定数
        /// </summary>
        public static readonly Vector3 BlockSize = Vector3.one * BlockScale;
        
        /// <summary>
        /// Blockの半分の大きさを表す定数
        /// </summary>
        public static readonly Vector3 BlockSizeHalf = Vector3.one * BlockScaleHalf;
    }
}