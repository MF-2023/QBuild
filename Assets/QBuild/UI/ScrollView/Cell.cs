using UnityEngine;

namespace QBuild.UI
{
    public abstract class Cell<TItemData> : MonoBehaviour
    {
        public int Index { get; set; } = -1;
        public virtual bool IsVisible => gameObject.activeSelf;
        public virtual void Initialize() { }
        public virtual void SetVisible(bool visible) => gameObject.SetActive(visible);
        public abstract void UpdateContent(TItemData itemData);
        public abstract void UpdatePosition(float position);

    }
}