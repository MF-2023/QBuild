using QBuild.UI;
using UnityEngine;

namespace QBuild.Part.HolderView
{
    class PartSlotView : Cell
    {
        [SerializeField] Animator animator = default;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        public override void UpdateContent(object itemData)
        {
            //message.text = itemData.Message;
        }


        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            var rect = GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(position, 0.5f);
            rect.anchorMin = new Vector2(position, 0.5f);
        }

        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}