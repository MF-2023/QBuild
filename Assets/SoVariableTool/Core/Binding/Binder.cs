using System.Collections.Generic;
using UnityEngine;

namespace SoVariableTool.Binding
{
    public class Binder : MonoBehaviour
    {
        [SerializeField] private List<Bindable> _bindables = new();

        private Bind _bind;

        private bool _isInitialized;

        private void Awake()
        {
            _bind = new Bind();
        }

        private void OnEnable()
        {
            AddBindablesToBind();
        }

        private void OnDisable()
        {
            RemoveBindablesFromBind();
        }


        private void InitializeBindables()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            _bindables.RemoveAll(bindable => bindable == null);

            foreach (var bindable in _bindables)
            {
                bindable.Owner = gameObject;
                bindable.Initialize();
            }
        }

        private void AddBindablesToBind()
        {
            if (_bind == null) return;
            InitializeBindables();

            foreach (var bindable in _bindables)
            {
                _bind.AddBindable(bindable);
                bindable.StartTick();
            }
        }

        private void RemoveBindablesFromBind()
        {
            if (_bind == null) return;
            for (var i = _bindables.Count - 1; i >= 0; i--)
            {
                var bindable = _bindables[i];
                if (bindable == null) continue;
                _bind.RemoveBindable(bindable);
                bindable.StopTick();
                _bind.RemoveBindable(bindable);
            }
        }
    }
}