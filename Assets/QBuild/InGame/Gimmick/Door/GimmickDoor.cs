using DG.Tweening;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickDoor : BaseGimmick
    {
        [SerializeField] private bool _isOpened = false;
        [SerializeField] private float _height;

        Tweener doorAnimation;
        public override void Active()
        {
            Open();
        }

        public override void Disable()
        {
        }

        public void Open()
        {
            if (_isOpened) return;
            doorAnimation = transform.DOMoveY(_height, 1).SetRelative(true);
            _isOpened = true;
        }

        public void Close()
        {
            if (!_isOpened) return;
            doorAnimation = transform.DOMoveY(-_height, 1).SetRelative(true);
            _isOpened = false;
        }
    }
}