using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QBuild.Part.HolderView
{
    public class PartHolderView : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;
        private int _pickIndex = 0;

        public void SetPartIcon(int index, PartIcon partIcon)
        {
            _images[index].enabled = true;
            _images[index].sprite = partIcon.Sprite;
        }

        public void SetQuantity(int index, int quantity)
        {
        }

        public void SetEmpty(int index)
        {
            _images[index].enabled = false;
        }

        public void Pick(int index)
        {
            SetScaleDown(_pickIndex);
            SetScaleUp(index);
            
            _pickIndex = index;
        }

        public void SetScaleUp(int index)
        {
            _images[index].transform.parent.parent.localScale = Vector3.one * 1.2f;
        }

        public void SetScaleDown(int index)
        {
            _images[index].transform.parent.parent.localScale = Vector3.one;
        }
    }
}