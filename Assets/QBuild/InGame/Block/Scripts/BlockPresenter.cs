using VContainer;
using VContainer.Unity;

namespace QBuild
{
    public class BlockPresenter : IInitializable
    {
        [Inject]
        public BlockPresenter(BlockFactory blockFactory, BlockStore blockStore)
        {
            _blockFactory = blockFactory;
            _blockStore = blockStore;
        }

        public void Initialize()
        {
            _blockFactory.OnBlockCreated += _blockStore.AddBlock;
        }

        private readonly BlockFactory _blockFactory;
        private readonly BlockStore _blockStore;
    }
}