using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/QBuild/StageEditor/SnapBehaviorObject")]
public class SnapBehaviorObject : ScriptableObject
{
    [SerializeField] private List<GameObject> _snapBehaviorObjects;
    public List<GameObject> GetSnapBehaviorObjects() => _snapBehaviorObjects;
}