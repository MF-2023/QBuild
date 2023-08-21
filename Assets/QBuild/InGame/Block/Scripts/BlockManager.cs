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
        private void Inject(StageFactory factory,StageScriptableObject stageScriptableObject,IBlockParentObject blockParentObject,IMinoFactory minoFactory,MinoTypeList minoTypeList)
        {
            Debug.Log("Inject BlockManager");
            _stageFactory = factory;
            _stageScriptableObject = stageScriptableObject;
            _blockParentObject = blockParentObject;
            _minoFactory = minoFactory;
            _minoTypeList = minoTypeList;
        }
        
        public void OnStartGame()
        {
            CreatePolyomino();
        }

        private void CreatePolyomino()
        {
            var minoType = _minoTypeList.NextGenerator();
            _minoFactory.CreateMino(minoType, _stageScriptableObject.MinoSpawnPosition,_blockParentObject.Transform);
        }
        [SerializeField] private GameObject _floorParent;

        private StageScriptableObject _stageScriptableObject;
        private IBlockParentObject _blockParentObject;
        private IMinoFactory _minoFactory;
        private MinoTypeList _minoTypeList;
        private StageFactory _stageFactory;
    }
}