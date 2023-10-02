using System;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Part
{
    [RequireComponent(typeof(Connector))]
    public class PartView : MonoBehaviour
    {
        private Connector _connector;

        public IEnumerable<Vector3> OnGetConnectPoints()
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }
            return _connector.ConnectPoints();
        }
    }
}