using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.UI.ScrollView
{
    public class ScrollView : MonoBehaviour
    {
        private IList itemsSource;
        private VisualElement itemTemplate;
        public int _selectedIndex = -1;
        public event Action<int> OnCellClicked;
    }
}