using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.UI.ScrollView
{
    public class ExtensionScrollView : UnityEngine.UIElements.ScrollView
    {
        public new class UxmlFactory : UxmlFactory<ExtensionScrollView, UxmlTraits>
        {
        }
    }
}