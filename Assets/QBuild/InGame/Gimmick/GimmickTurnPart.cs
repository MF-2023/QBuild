using System;
using QBuild.Part;
using QBuild.Utilities;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickTurnPart : BaseGimmick
    {
        [SerializeField] private PartView _partView;

        public override void Active()
        {
            Debug.Log("Rotate");
            _partView.Turn(new ShiftDirectionTimes(1));
        }

        public override void Disable()
        {
        }
    }
}