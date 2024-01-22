using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Part.HolderView;
using UnityEngine;
using VContainer;

namespace QBuild.Part.Presenter
{
    public class HolderPresenter
    {
        [Inject]
        public HolderPresenter(PartHolderView partHolderView)
        {
            _partHolderView = partHolderView;
        }


        public void Initialize()
        {
        }

        public void Bind(IPartsHoldable holder)
        {
            _holder = holder;
            
            _holder.OnChangedSelect += OnSelectChanged;

            _holder.OnUsePart += OnUsedPart;
            _holder.OnSlotsUpdated += OnSlotsUpdated;
        }

        private void OnUsedPart(object sender, HolderUseEventArgs e)
        {
            if (e.Slot is not QuantitySlot quantitySlot) return;
            if (sender is not PlayerPartHolder playerPartHolder) return;
            
            SetIcon(e.CurrentIndex, quantitySlot.Quantity == 0 ? null : e.Part);
            SetQuantity(e.CurrentIndex, quantitySlot.Quantity);
        }

        private void SetIcon(int holdersIndex, BlockPartScriptableObject part)
        {
            if (part == null)
            {
                _partHolderView.SetEmpty(holdersIndex);
                return;
            }

            _partHolderView.SetPartIcon(holdersIndex, part.PartIcon);
        }

        private void SetQuantity(int holdersIndex, int quantity)
        {
            _partHolderView.SetQuantity(holdersIndex, quantity);
        }

        private void OnSelectChanged(object sender, HolderSelectChangeEventArgs e)
        {
            _partHolderView.Pick(e.CurrentIndex);
        }

        private void OnSlotsUpdated(object sender, HolderSlotsUpdateEventArgs args)
        {
            _partHolderView.SetSize(_holder.Slots.Count());

            for (var i = 0; i < _holder.Slots.Count(); i++)
            {
                var slot = _holder.Slots.ElementAt(i);
                SetIcon(i, slot.GetPart());
                if (slot is QuantitySlot quantitySlot)
                {
                    SetQuantity(i, quantitySlot.Quantity);
                }
            }

            _partHolderView.Pick(_holder.CurrentPartIndex);
        }
        
        private IPartsHoldable _holder;
        private PartHolderView _partHolderView;
    }
}