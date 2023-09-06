using QBuild.Mino;
using SherbetInspector.Core.Attributes;
using UnityEngine;
using VContainer;

namespace QBuild.DebugSystem
{
    public class StageTemplateGenerator : MonoBehaviour
    {
        [Inject]
        public void Inject(IMinoFactory minoFactory,MinoService minoService)
        {
            _minoFactory = minoFactory;
            _minoService = minoService;
            
        }
        
        [Button]
        private void GenerateTemplate()
        {
            if (_isGenerated)
            {
                Debug.Log("既に生成済みです");
                return;
            }
            if(_minoFactory == null)
            {
                Debug.Log("MinoFactoryがInjectされていません");
                return;
            }
            var minoInfos = _template.GetPlacedMinoInfos();
            foreach (var minoInfo in minoInfos)
            {
                var mino = _minoFactory.CreateMinoEventSkip(minoInfo.MinoType, minoInfo.Position, null);
                if (_minoService.JointMino(mino))
                {
                    continue;
                }

                mino.Place();
            }
            
            _isGenerated = true;
        }
        
        [SerializeField] private MinoStageTemplate _template;
        private IMinoFactory _minoFactory;
        private MinoService _minoService;
        private bool _isGenerated = false;
    }
}