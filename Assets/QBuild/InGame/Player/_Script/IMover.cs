using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  QBuild.Player
{
    public interface IMover
    {
        
        public void OnMoverEnter() {}

        public void OnMoverExit() {}
        public void SetMoverVelocity(Vector3 velocity){}
    }
}
