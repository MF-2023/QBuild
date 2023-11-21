using System;
using UnityEngine;

namespace QBuild.Gimmick.Effector
{
    public abstract class BaseEffectorData
    {
        public GameObject Target { get; set; }
    }
    

    public interface IEffector
    {
        public void Execute(BaseEffectorData e);
    }
}