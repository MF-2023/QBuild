using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QBuild.Part.HolderView
{
    public class PartHolderView : MonoBehaviour
    {
        public void SetPartIcon(int index,PartIcon partIcon)
        {
            _images[index].sprite = partIcon.Sprite;
        }
        public void SetQuantity(int index,int quantity)
        {
        }

        public void SetScaleUp(int index)
        {
            _images[index].transform.parent.parent.localScale = Vector3.one * 1.2f;
        }
        
        public void SetScaleDown(int index)
        {
            _images[index].transform.parent.parent.localScale = Vector3.one;
        }
        
        [SerializeField] private List<Image> _images;
    }
}