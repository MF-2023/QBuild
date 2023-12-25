using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.UI;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace QBuild.Part.HolderView
{
    public class PartHolderView : MonoBehaviour
    {


        private readonly  List<SlotData> _slots = new();
        [SerializeField] private HolderScrollView _holderScrollView;
        private int _pickIndex = 0;

        public void SetSize(int size)
        {
            _slots.Clear();
            for (var i = 0; i < size; i++)
            {
                _slots.Add(new SlotData());
            }

            Debug.Log($"SetSize {size}");
            UpdateData();
        }

        public void SetPartIcon(int index, PartIcon partIcon)
        {
            _slots[index].Sprite = partIcon.Sprite;

            UpdateData();
        }

        public void SetQuantity(int index, int quantity)
        {
            _slots[index].Quantity = quantity;

            UpdateData();
        }

        public void SetEmpty(int index)
        {
            _slots[index] ??= new SlotData();
            _slots[index].Sprite = null;
            _slots[index].Quantity = 0;

            UpdateData();
        }

        public void Pick(int index)
        {
            _pickIndex = index;
            _holderScrollView.ScrollTo(index);
        }
        
        private void UpdateData()
        {
            _holderScrollView.UpdateData(_slots.Where(x => x.Quantity > 0).ToList());
        }
    }
}