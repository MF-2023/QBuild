using QBuild.Mino;
using SherbetInspector.Core.Attributes;
using UnityEngine;
using VContainer;

namespace QBuild.DebugSystem
{
    public class StageTemplateGenerator : MonoBehaviour
    {
        [Inject]
        public void Inject(MinoFactory minoFactory)
        {
            _minoFactory = minoFactory;
        }
        
        [Button]
        private void GenerateTemplate()
        {
            if (_isGenerated) return;
            var minoInfos = _template.GetPlacedMinoInfos();
            foreach (var minoInfo in minoInfos)
            {
                _minoFactory.CreateMino(minoInfo.MinoType, minoInfo.Position, null);
            }
            
            _isGenerated = true;
        }
        
        [SerializeField] private MinoStageTemplate _template;
        private IMinoFactory _minoFactory;
        private bool _isGenerated = false;
    }
}