#if UNITY_EDITOR
using QBuild.Utilities;
#endif

using UnityEditor;

namespace QBuild
{
    [InitializeOnLoad]
    public static class BlockManagerBind
    {
        private static BlockManager _blockManager;

        static BlockManagerBind()
        {
#if UNITY_EDITOR
            PlaymodeStateObserver.OnPressedEndButton += Clear;
#endif
        }

        public static void Init(BlockManager bm)
        {
            _blockManager = bm;
        }

        private static void Clear()
        {
            if (_blockManager != null)
                _blockManager.Clear();
        }
    }
}