using System.Collections.Generic;
using UnityEngine;
using QBuild.UI;

namespace QBuild.Part.HolderView
{
    class HolderScrollView : ScrollView
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        protected override GameObject CellPrefab => cellPrefab;

        protected override void Initialize()
        {
            base.Initialize();
            scroller.OnValueChanged(UpdatePosition);
        }

        public void UpdateData(IList<object> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }
    }
}