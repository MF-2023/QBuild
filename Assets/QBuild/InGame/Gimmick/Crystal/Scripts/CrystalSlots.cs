using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QBuild.Gimmick.Crystal
{
    public class CrystalSlots : MonoBehaviour
    {
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Sprite _crystalSprite;
        [SerializeField] private GameObject _crystalSlotPrefab;

        [SerializeField] private float _margin;

        private readonly List<Image> _slots = new();
        private int _crystalCount;
        private int MaxCrystalCount => _slots.Count;

        public void SetMaxCrystalCount(int maxCrystalCount)
        {
            if (MaxCrystalCount != 0) throw new InvalidOperationException("MaxCrystalCount is already set.");
            if (maxCrystalCount < 0) throw new ArgumentOutOfRangeException(nameof(maxCrystalCount));

            for (var i = 0; i < maxCrystalCount; i++)
            {
                var slot = Instantiate(_crystalSlotPrefab, transform);
                var image = slot.GetComponent<Image>();
                image.sprite = _emptySprite;
                var rectTransform = slot.GetComponent<RectTransform>();
                var x = (image.rectTransform.sizeDelta.x + _margin) * i;
                rectTransform.anchoredPosition = new Vector2(x, 0);
                _slots.Add(image);
            }
        }

        public void OnGetCrystal()
        {
            if (_crystalCount >= MaxCrystalCount) return;
            _slots[_crystalCount].sprite = _crystalSprite;
            _crystalCount++;
        }
    }
}