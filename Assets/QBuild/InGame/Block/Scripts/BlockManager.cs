using QBuild.Mino;
using QBuild.Stage;
using SherbetInspector.Core.Attributes;
using UnityEngine;
using VContainer;

namespace QBuild
{
    public class BlockManager : MonoBehaviour
    {
        private void Start()
        {
            _stageFactory.CreateFloor(_floorParent);
        }

        [Inject]
        private void Inject(StageFactory factory, StageScriptableObject stageScriptableObject,
            IBlockParentObject blockParentObject, IMinoFactory minoFactory, MinoTypeList minoTypeList,
            BlockService blockService, BlockFactory blockFactory)
        {
            Debug.Log("Inject BlockManager");
            _stageFactory = factory;
            _stageScriptableObject = stageScriptableObject;
            _blockParentObject = blockParentObject;
            _minoFactory = minoFactory;
            _minoTypeList = minoTypeList;
            _blockService = blockService;
            _blockFactory = blockFactory;
        }

        public void OnStartGame()
        {
            CreatePolyomino();
        }

        public bool TryGenerateBlock(BlockType blockType, Vector3Int position, out Block block)
        {
            block = null;
            if (!_blockService.CanPlace(position)) return false;
            block = _blockFactory.CreateBlock(blockType, position, _blockParentObject.Transform);
            return true;
        }

        public bool TryGetBlock(Vector3Int position, out Block block)
        {
            return _blockService.TryGetBlock(position, out block);
        }

        private void CreatePolyomino()
        {
            var minoType = _minoTypeList.NextGenerator();
            _minoFactory.CreateMino(minoType, _stageScriptableObject.MinoSpawnPosition, _blockParentObject.Transform);
        }

        [SerializeField] private GameObject _floorParent;

        private StageScriptableObject _stageScriptableObject;
        private IBlockParentObject _blockParentObject;
        private IMinoFactory _minoFactory;
        private MinoTypeList _minoTypeList;
        private StageFactory _stageFactory;
        private BlockService _blockService;
        private BlockFactory _blockFactory;
    }
}