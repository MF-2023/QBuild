using QBuild.Mino;
using QBuild.Stage;
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
        private void Inject(StageFactory factory,StabilityCalculator stabilityCalculator)
        {
            Debug.Log("Inject BlockManager");
            _stageFactory = factory;
        }
        
        [SerializeField] private GameObject _floorParent;

        private StageFactory _stageFactory;
    }
}