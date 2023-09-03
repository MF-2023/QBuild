using System;
using QBuild.Const;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.Camera
{
    [CreateAssetMenu(fileName = "CameraVariable", menuName = EditorConst.VariablePrePath + "CameraVariable",
        order = EditorConst.OtherOrder)]
    public class CameraScriptableObject : ScriptableObject
    {
        public IReadOnlyReactiveProperty<float> HeightOffset => _heightOffset;
        public IReadOnlyReactiveProperty<float> Distance => _distance;
        
        public IReadOnlyReactiveProperty<float> angleOffset => _angleOffset;
        
        public IReadOnlyReactiveProperty<Vector3> CenterOffset => _centerOffset;
        
        [FormerlySerializedAs("_height")] [SerializeField] private FloatReactiveProperty _heightOffset = new(10f);
        [SerializeField] private FloatReactiveProperty _distance = new(15f);
        [SerializeField] private FloatReactiveProperty _angleOffset = new(0f);
        [SerializeField] private Vector3ReactiveProperty _centerOffset = new(Vector3.zero);
        
    }
}