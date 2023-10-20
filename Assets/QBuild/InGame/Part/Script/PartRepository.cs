using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Part
{
    public class PartRepository : MonoBehaviour
    {
        [SerializeField] private List<PartView> _partViews = new();
        
        public void AddPart(PartView partView)
        {
            _partViews.Add(partView);
        }
        
        public void AllDestroy()
        {
            foreach (var partView in _partViews)
            {
                Destroy(partView.gameObject);
            }
            _partViews.Clear();
        }
        
    }
}