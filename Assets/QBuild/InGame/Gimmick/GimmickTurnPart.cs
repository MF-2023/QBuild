using System;
using QBuild.Part;
using QBuild.Utilities;
using SherbetInspector.Core.Attributes;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickTurnPart : BaseGimmick
    {
        [SerializeField] private PartView _partView;

        [Button]
        public override void Active()
        {
            _partView.Turn(new ShiftDirectionTimes(1));
        }

        public override void Disable()
        {
        }
    }
}