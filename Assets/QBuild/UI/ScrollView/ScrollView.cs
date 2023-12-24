using System.Collections.Generic;
using UnityEngine;

namespace QBuild.UI
{
    public abstract class ScrollView : MonoBehaviour
    {
        [SerializeField] protected float _interval = 0.5f;
        [SerializeField] protected bool _loop = false;
        [SerializeField] protected Transform _cellContainer = default;
        
        protected readonly IList<Cell> _pool = new List<Cell>();
        protected bool _initialized = false;
        protected float _currentPosition = 0f;
        protected IList<object> ItemsSource { get; set; } = new List<object>();
        protected abstract GameObject CellPrefab { get; }

        protected virtual void Initialize()
        {
        }

        protected virtual void UpdateContents(IList<object> items)
        {
            ItemsSource = items;
            Refresh();
        }

        protected virtual void Relayout() => UpdatePosition(_currentPosition, false);

        protected virtual void Refresh() => UpdatePosition(_currentPosition, true);
        protected virtual void UpdatePosition(float position) => UpdatePosition(position, false);

        void UpdatePosition(float position, bool forceRefresh)
        {
            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }

            _currentPosition = position;
            var p = position;
            var firstIndex = Mathf.CeilToInt(p);
            var firstPosition = (Mathf.Ceil(p) - p) * _interval;

            if (firstPosition + _pool.Count * _interval < 1f)
            {
                ResizePool(firstPosition);
            }

            Debug.Log($"UpdatePosition: {firstPosition}, {firstIndex}, {forceRefresh}");
            UpdateCells(firstPosition, firstIndex, forceRefresh);
        }

        void ResizePool(float firstPosition)
        {
            var addCount = Mathf.CeilToInt((1f - firstPosition) / _interval) - _pool.Count;
            for (var i = 0; i < addCount; i++)
            {
                var cell = Instantiate(CellPrefab, _cellContainer).GetComponent<Cell>();
                Debug.Log("Create cell");
                if (cell == null)
                {
                    Debug.LogError("Cell prefab must have Cell component.", this);
                    return;
                }

                //cell.SetContext(Context);
                cell.Initialize();
                cell.SetVisible(false);
                _pool.Add(cell);
            }
        }

        void UpdateCells(float firstPosition, int firstIndex, bool forceRefresh)
        {
            for (var i = 0; i < _pool.Count; i++)
            {
                var index = firstIndex + i;
                var position = firstPosition + i * _interval;
                var cell = _pool[CircularIndex(index, _pool.Count)];

                if (_loop)
                {
                    index = CircularIndex(index, ItemsSource.Count);
                }

                if (index < 0 || index >= ItemsSource.Count || position > 1f)
                {
                    cell.SetVisible(false);
                    continue;
                }

                if (forceRefresh || cell.Index != index || !cell.IsVisible)
                {
                    cell.Index = index;
                    cell.SetVisible(true);
                    cell.UpdateContent(ItemsSource[index]);
                }

                cell.UpdatePosition(position);
            }
        }

        int CircularIndex(int i, int size) => size < 1 ? 0 : i < 0 ? size - 1 + (i + 1) % size : i % size;

#if UNITY_EDITOR
        bool cachedLoop;
        float cachedCellInterval;

        void LateUpdate()
        {
            if (cachedLoop != _loop ||
                cachedCellInterval != _interval)
            {
                cachedLoop = _loop;
                cachedCellInterval = _interval;
                UpdatePosition(_currentPosition);
            }
        }
#endif
    }
}