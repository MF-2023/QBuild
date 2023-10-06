﻿using System;
using System.Collections.Generic;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Part
{
    
    [CreateAssetMenu(fileName = "BlockPart", menuName = EditorConst.ScriptablePrePath + "Part", order = 0)]
    public class BlockPartScriptableObject : ScriptableObject
    {
        public PartView PartPrefab => _partPrefab;
        public PartIcon PartIcon => _partIcon;
        
        public void SetPartPrefab(PartView partPrefab)
        {
            _partPrefab = partPrefab;
        }
        [SerializeField] private PartView _partPrefab;
        [SerializeField] private PartIcon _partIcon;
    }
}