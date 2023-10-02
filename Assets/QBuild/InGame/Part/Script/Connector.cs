using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QBuild.Part
{
    [Serializable]
    public class ConnectPoint
    {
        [SerializeField] private Vector3 _position;
        public Vector3 Position => _position;
    }
    
    public class Connector : MonoBehaviour
    {
        [SerializeField] private List<ConnectPoint> _connectPoints;
        
        public IEnumerable<Vector3> ConnectPoints() => _connectPoints.Select(p => p.Position);
    }
}