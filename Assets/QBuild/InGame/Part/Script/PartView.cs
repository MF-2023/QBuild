using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Utilities;
using UnityEngine;

namespace QBuild.Part
{
    [RequireComponent(typeof(Connector))]
    public class PartView : MonoBehaviour
    {
        private Connector _connector;

        public DirectionFRBL Direction { get; set; } = DirectionFRBL.Forward;


        public IEnumerable<Vector3> OnGetConnectPoints()
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            return _connector.ConnectPoints();
        }

        public IEnumerable<ConnectPoint.Magnet> OnGetMagnets()
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            return _connector.ConnectMagnet();
        }

        public bool TryGetConnectPoint(DirectionFRBL direction, out Vector3 position)
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            return _connector.TryGetConnectPoint(direction, out position);
        }

        public bool HasDirection(DirectionFRBL direction)
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            return _connector.HasDirection(direction);
        }

        public void SetCanConnect(DirectionFRBL dir, bool canConnect)
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }
            var partDir = Direction;
            Debug.Log($"calc ${Direction} ${dir}");
            var shiftedDir = DirectionUtilities.CalcDirectionFRBL(Direction, dir);

            while (partDir != DirectionFRBL.Forward)
            {
                dir = dir.TurnRight();
                partDir = partDir.TurnRight();
            }
            
            Debug.Log($"${dir} ${shiftedDir}");

            _connector.SetCanConnect(shiftedDir, canConnect);
        }
    }
}