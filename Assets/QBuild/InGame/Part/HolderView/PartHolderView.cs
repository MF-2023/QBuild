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
        [SerializeField] private List<Image> _images;
    }
}