using System;
using SherbetInspector.Core.Attributes;
using UnityEngine;

namespace QBuild.Part.Event
{
    
    public interface IPartListEvent
    {
        public void Execute(ProcessCalcParameter calcParameter, BlockPartScriptableObject target);
    }
    
    [Serializable, TypeLabel("確率リセット")]
    public class ResetEvent : IPartListEvent
    { 
        public void Execute(ProcessCalcParameter calcParameter, BlockPartScriptableObject target)
        {
            calcParameter.PartListScriptableObject.ResetProbability(target);
        }
    }
    
    [Serializable, TypeLabel("確定演出")]
    public class DetermineEvent : IPartListEvent
    { 
        public void Execute(ProcessCalcParameter calcParameter, BlockPartScriptableObject target)
        {
            calcParameter.PartListScriptableObject.AddReservationPart(target);
        }
    }
}