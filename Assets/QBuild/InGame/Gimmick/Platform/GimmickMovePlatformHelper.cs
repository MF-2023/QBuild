using System;
using System.Collections;
using System.Collections.Generic;
using QBuild.Player;
using UnityEngine;

namespace  QBuild.Gimmick
{
    public class GimmickMovePlatformHelper : MonoBehaviour
    {
        public Action<IMover> AddMoverEvent;
        public Action<IMover> RemoveMoverEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<IMover>(out IMover adapter))
            {
                adapter.OnMoverEnter();
                AddMoverEvent?.Invoke(adapter);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent<IMover>(out IMover adapter))
            {
                adapter.OnMoverExit();
                RemoveMoverEvent?.Invoke(adapter);
            }
        }
    }
}