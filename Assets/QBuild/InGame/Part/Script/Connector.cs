using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Utilities;
using UnityEngine;

namespace QBuild.Part
{
    [Serializable]
    public class PartConnectPoint
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private DirectionFRBL _direction;
        [SerializeField] private bool _canConnect = true;
        public Vector3 Position => _position;
        public DirectionFRBL Direction => _direction;
        public bool CanConnect => _canConnect;

        public void SetCanConnect(bool canConnect)
        {
            _canConnect = canConnect;
        }

        public struct Magnet
        {
            public Vector3 Position;
            public DirectionFRBL Direction;
            public bool CanConnect;
        }
    }

    public class Connector : MonoBehaviour
    {
        [SerializeField] private List<PartConnectPoint> _connectPoints;

        public IEnumerable<Vector3> ConnectPoints() => _connectPoints.Select(p => p.Position);

        public IEnumerable<PartConnectPoint.Magnet> ConnectMagnet()
        {
            return _connectPoints.Select(p => new PartConnectPoint.Magnet
            {
                Position = p.Position,
                Direction = p.Direction,
                CanConnect = p.CanConnect
            });
        }

        public bool TryGetConnectPoint(DirectionFRBL direction, out Vector3 position)
        {
            var connectPoint = _connectPoints.FirstOrDefault(x => x.Direction == direction && x.CanConnect);
            if (connectPoint == null)
            {
                position = Vector3.zero;
                return false;
            }

            position = connectPoint.Position;
            return true;
        }

        public bool HasDirection(DirectionFRBL direction)
        {
            return _connectPoints.Any(x => x.Direction == direction);
        }

        public void SetCanConnect(DirectionFRBL dir, bool canConnect)
        {
            var connectPoint = _connectPoints.FirstOrDefault(x => x.Direction == dir);

            if (!gameObject.activeInHierarchy) return;
            
            connectPoint?.SetCanConnect(canConnect);
        }
    }
}