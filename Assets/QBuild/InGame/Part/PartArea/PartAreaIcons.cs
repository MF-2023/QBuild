using System.Collections.Generic;
using QBuild.Const;
using QBuild.Utilities;
using UnityEngine;

namespace QBuild.Part
{
    [CreateAssetMenu(fileName = "PartAreaIcons", menuName = EditorConst.VariablePrePath + "PartAreaIcon", order = EditorConst.OtherOrder)]
    public class PartAreaIcons : ScriptableObject
    {
        
        public Sprite GetIcon(DirectionFRBL direction)
        {
            return direction switch
            {
                DirectionFRBL.Forward => ForwardAreaIcon,
                DirectionFRBL.Left => LeftAreaIcon,
                DirectionFRBL.Right => RightAreaIcon,
                DirectionFRBL.Back => BackAreaIcon,
                _ => throw new KeyNotFoundException($"DirectionFRBL {direction} not found"),
            };
        }
        
        
        
        public Sprite ForwardAreaIcon => _forwardAreaIcon;
        public Sprite LeftAreaIcon => _leftAreaIcon;
        public Sprite RightAreaIcon => _rightAreaIcon;
        public Sprite BackAreaIcon => _backAreaIcon;
        
        [SerializeField] private Sprite _forwardAreaIcon;
        [SerializeField] private Sprite _leftAreaIcon;
        [SerializeField] private Sprite _rightAreaIcon;
        [SerializeField] private Sprite _backAreaIcon;
    }
}