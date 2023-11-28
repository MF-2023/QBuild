using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickRotateBar : BaseGimmick
    {
        private enum BarActionType
        {
            None,
            OnSwitch,
            ReverseSwitch,
        }
        [Tooltip("アクション時の振る舞い")]
        [SerializeField]
        private BarActionType _actionType;

        [Tooltip("オンの間は回転する")]
        [SerializeField]
        private bool _isOn;
        [Tooltip("オン：右回転、オフ：左回転")]
        [SerializeField] private bool _isReverse;
        
        public override void Active()
        {
            switch (_actionType)
            {
                case BarActionType.None:
                    break;
                case BarActionType.OnSwitch:
                    SwitchedOn();
                    break;
                case BarActionType.ReverseSwitch:
                    SwitchedReverse();
                    break;
                default:
                    break;
            }
        }

        public override void Disable()
        {
            
        }
        
        
        public void SwitchedOn()
        {
            _isOn = !_isOn;
        }
        public void SwitchedReverse()
        {
            _isReverse = !_isReverse;
        }
    }
}