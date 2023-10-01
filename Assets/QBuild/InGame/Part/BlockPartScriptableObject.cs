﻿using QBuild.Const;
using UnityEngine;

namespace QBuild.Part
{
    [CreateAssetMenu(fileName = "BlockPart", menuName = EditorConst.ScriptablePrePath + "Part", order = 0)]
    public class BlockPartScriptableObject : ScriptableObject
    {
        [SerializeField] private PartView _partPrefab;
        
    }
}