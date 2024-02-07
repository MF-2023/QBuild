using UnityEngine;
using UnityEngine.UI;

namespace QBuild
{
    public class HealthBarElement : MonoBehaviour
    {
        [SerializeField] private Image _healthIcon;
        
        public void SetIcon(Sprite sprite)
        {
            _healthIcon.sprite = sprite;
        }
    }
}