﻿using System;
using System.Collections.Generic;

namespace QBuild.Part
{
    public sealed class HolderSelectChangeEventArgs : EventArgs
    {
        public BlockPartScriptableObject Part { get; private set; }
        public BaseSlot Slot { get; private set; }
        public int CurrentIndex { get; private set; }
        public int PrevIndex { get; private set; }

        public HolderSelectChangeEventArgs(BlockPartScriptableObject part, BaseSlot slot, int currentIndex,
            int prevIndex)
        {
            Part = part;
            Slot = slot;
            CurrentIndex = currentIndex;
            PrevIndex = prevIndex;
        }
    }

    public delegate void HolderSelectChangedEventHandler(object sender, HolderSelectChangeEventArgs args);

    public sealed class HolderUseEventArgs : EventArgs
    {
        public BlockPartScriptableObject Part { get; private set; }
        public int CurrentIndex { get; private set; }
        public BaseSlot Slot { get; private set; }

        public HolderUseEventArgs(BlockPartScriptableObject part, BaseSlot slot, int currentIndex)
        {
            Part = part;
            Slot = slot;
            CurrentIndex = currentIndex;
        }
    }

    public delegate void HolderUsedEventHandler(object sender, HolderUseEventArgs args);


    public sealed class HolderSlotsUpdateEventArgs : EventArgs
    {
        public IEnumerable<BaseSlot> Slots { get; private set; }

        public HolderSlotsUpdateEventArgs(IEnumerable<BaseSlot> slots)
        {
            Slots = slots;
        }
    }

    public delegate void HolderSlotsUpdatedEventHandler(object sender, HolderSlotsUpdateEventArgs args);

    public interface IPartsHoldable
    {
        event HolderUsedEventHandler OnUsePart;
        event HolderSelectChangedEventHandler OnChangedSelect;

        event HolderSlotsUpdatedEventHandler OnSlotsUpdated;
        
        IEnumerable<BaseSlot> Slots { get; }

        int CurrentPartIndex { get; }
    }
}