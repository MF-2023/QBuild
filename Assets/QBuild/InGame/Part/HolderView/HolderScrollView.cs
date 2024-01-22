using System.Collections.Generic;
using UnityEngine;
using QBuild.UI;

namespace QBuild.Part.HolderView
{
    public class SlotData
    {
        public Sprite Sprite;
        public int Quantity;
    }
    
    class HolderScrollView : ScrollView<SlotData>
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        protected override GameObject CellPrefab => cellPrefab;

        protected override void Initialize()
        {
            base.Initialize();
            scroller.OnValueChanged(UpdatePosition);
        }

        public void UpdateData(IList<SlotData> items)
        {
            this._loop = items.Count > 1;
            if (!this._loop) scroller.ContentMovementType = MovementType.Clamped;
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }
        
        public void ScrollTo(int index)
        {
            scroller.ScrollTo(index, 0.35f);
        }
    }
}