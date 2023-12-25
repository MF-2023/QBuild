using QBuild.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QBuild.Part.HolderView
{
    class PartSlotView : Cell<SlotData>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _quantityText;
        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("PartSlotScrollAnimation");
        }

        public override void UpdateContent(SlotData itemData)
        {
            _image.sprite = itemData.Sprite;
            _quantityText.text = itemData.Quantity.ToString();
        }


        public override void UpdatePosition(float position)
        {
            _currentPosition = position;
            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }

            animator.speed = 0;
        }

        private float _currentPosition = 0;

        private void OnEnable() => UpdatePosition(_currentPosition);
    }
}