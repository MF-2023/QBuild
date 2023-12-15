using QBuild.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class MoverAdapter : MonoBehaviour, IMover
    {
        public void AddMoverPosition(Vector3 addPos)
        {
            transform.position += addPos;
        }

        public void OnMoverEnter()
        {
        }

        public void OnMoverExit()
        {
        }

        public void SetMoverVelocity(Vector3 velocity)
        {
        }
    }
}