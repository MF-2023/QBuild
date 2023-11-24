using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  QBuild.Player
{
    public interface IMover
    {
        public Vector3 CurrentMoverVelo { get; set; }
        public bool OnMover { get; set; }
    }
}
