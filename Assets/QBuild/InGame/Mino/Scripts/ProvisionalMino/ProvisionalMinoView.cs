using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace QBuild.Mino.ProvisionalMino
{
    public class ProvisionalMinoView
    {
        [Inject]
        public ProvisionalMinoView(ProvisionalMinoSetting setting,IBlockParentObject blockParentObject)
        {
            _setting = setting;
            _blockParentObject = blockParentObject;
        }
        
        public void SetPosition(IEnumerable<Vector3Int>  positions)
        {
            _provisionalMino.ForEach(Object.Destroy);
            _provisionalMino.Clear();
            
            foreach (var position in positions)
            {
                var obj = Object.Instantiate(_setting.ProvisionalMinoPrefab, _blockParentObject.Transform);
                obj.transform.localPosition = position;
                _provisionalMino.Add(obj);
            }
        }
        
        private readonly List<GameObject> _provisionalMino = new();

        private readonly ProvisionalMinoSetting _setting;
        private readonly IBlockParentObject _blockParentObject;
    }
}