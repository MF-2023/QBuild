using System;
using UnityEngine;

namespace QBuild.Gimmick.TriggerPattern
{
    public interface ITrigger
    {
        public event Action OnActive;
        public event Action OnDisable;
        
        public void TriggerBind(GimmickTrigger gimmick);
    }
}