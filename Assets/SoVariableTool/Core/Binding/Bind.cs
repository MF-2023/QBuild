using System;
using System.Collections.Generic;
using System.Linq;

namespace SoVariableTool.Binding
{
    public class Bind
    {
        public Guid Guid { get; } = Guid.NewGuid();
        private readonly HashSet<Bindable> _bindables = new();
        private readonly HashSet<Guid> _bindablesGuid = new();
        public Bindable LastBindable { get; private set; }

        public void NotifyChange(Guid changedBindableGuid)
        {
            if (changedBindableGuid == Guid.Empty) return;

            if (!_bindablesGuid.Contains(changedBindableGuid)) return;

            if (_bindables.FirstOrDefault(b => b.Guid == changedBindableGuid) is not { } changedBindable) return;

            foreach (var bindable in _bindables)
            {
                if (bindable == changedBindable) continue;
                if (bindable.ConnectionType == ConnectionType.Sender) continue;
                bindable.OnValueChanged?.Invoke();
                Bindable.ProcessValue(this, changedBindable, bindable);
            }

            LastBindable = changedBindable;
        }

        public Bind AddBindable(Bindable bindable)
        {
            if (bindable == null) return this;

            if (bindable.Bind != null && bindable.Bind != this)
                bindable.Bind.RemoveBindable(bindable);

            _bindables.Add(bindable);
            _bindablesGuid.Add(bindable.Guid);

            bindable.Bind = this;
            bindable.RunOnBind();

            return this;
        }

        public Bind RemoveBindable(Bindable bindable)
        {
            if (bindable == null) return this;
            if (!_bindables.Contains(bindable)) return this;
            _bindables.Remove(bindable);
            _bindablesGuid.Remove(bindable.Guid);
            return this;
        }
    }
}