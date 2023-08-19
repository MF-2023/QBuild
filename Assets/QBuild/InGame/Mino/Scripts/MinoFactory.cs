using VContainer;

namespace QBuild.Mino
{
    public class MinoFactory
    {
        [Inject]
        public MinoFactory(BlockManager blockManager)
        {
            _blockManager = blockManager;
        }

        private BlockManager _blockManager;
    }
}