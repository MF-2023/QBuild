using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Part.HolderView;
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

            for (var i = 0; i < _holder.Slots.Count(); i++)
            {
                var slot = _holder.Slots.ElementAt(i);
                SetIcon(i, slot.GetPart());
                if (slot is QuantitySlot quantitySlot)
                {
                    SetQuantity(i, quantitySlot.Quantity);
                }
                
            }
        }

        private void OnUsedPart(object sender, HolderUseEventArgs e)
        {
            SetIcon(e.CurrentIndex, e.Part);
            if (e.Slot is QuantitySlot quantitySlot)
            {
                SetQuantity(e.CurrentIndex, quantitySlot.Quantity);
            }
        }

        private void SetIcon(int holdersIndex, BlockPartScriptableObject part)
        {
            _partHolderView.SetPartIcon(holdersIndex, part.PartIcon);
        }

        private void SetQuantity(int holdersIndex, int quantity)
        {
            _partHolderView.SetQuantity(holdersIndex, quantity);
        }

        private void OnSelectChanged(object sender, HolderSelectChangeEventArgs e)
        {
            _partHolderView.SetScaleUp(e.CurrentIndex);
            _partHolderView.SetScaleDown(e.PrevIndex);
        }

        private IPartsHoldable _holder;
        private PartHolderView _partHolderView;
    }
}