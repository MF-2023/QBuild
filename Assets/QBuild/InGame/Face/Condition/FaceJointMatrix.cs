﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SherbetInspector.Core.Attributes;
using SherbetInspector.Serializable;
using UnityEditor;
using UnityEngine;


namespace QBuild.Condition
{
    [Serializable]
    public class ConditionMap : SerializableDictionary<FaceScriptableObject,bool>
    {

    }
    [Serializable]
    public class ConditionMatrix : SerializableDictionary<FaceScriptableObject,ConditionMap>
    {

    }
    [CreateAssetMenu(fileName = "FaceJointMatrix", menuName = "Tools/QBuild/FaceConditionMatrix", order = 0)]
    public class FaceJointMatrix : ScriptableObject
    {
        [SerializeField] private List<FaceScriptableObject> faceScriptableObjects;
        
        [SerializeField] private ConditionMatrix conditionFaces;


        public IReadOnlyList<FaceScriptableObject> GetFaceTypes()
        {
            return faceScriptableObjects;
        }
        public ConditionMatrix GetMatrix()
        {
            return conditionFaces;
        }
        [Button]
        public void Refresh()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:FaceScriptableObject");
            if (guids.Length == 0)
            {
                throw new System.IO.FileNotFoundException("FaceScriptableObject does not found");
            }

            faceScriptableObjects = guids.Select(guid =>
                AssetDatabase.LoadAssetAtPath<FaceScriptableObject>(AssetDatabase.GUIDToAssetPath(guid))).ToList();
            {
                foreach (var faceScriptableObject in faceScriptableObjects)
                {
                    if (conditionFaces.ContainsKey(faceScriptableObject))
                    {
                    }
                    else
                    {
                        var conditions = new ConditionMap();
                        foreach (var scriptableObject in faceScriptableObjects) conditions.Add(scriptableObject, true);
                        conditionFaces.Add(faceScriptableObject, conditions);
                    }
                }
            }
            EditorUtility.SetDirty(this);
        }

        public bool GetCondition(FaceScriptableObject face1, FaceScriptableObject face2)
        {
            return conditionFaces[face1][face2];
        }

        public void SetCondition(FaceScriptableObject face1, FaceScriptableObject face2,bool flg)
        {
            conditionFaces[face1][face2] = flg;
            EditorUtility.SetDirty(this);
        }
    }
}