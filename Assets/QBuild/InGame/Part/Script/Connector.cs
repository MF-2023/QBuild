using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Part
{
    public class Connector : MonoBehaviour
    {
        [SerializeField] private List<Vector3> _connectPoints;
        
        public IEnumerable<Vector3> ConnectPoints() => _connectPoints;
    }
}