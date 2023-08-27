using System;
using QBuild.Const;
using UniRx;
using UnityEngine;

namespace QBuild.Camera
{
    [CreateAssetMenu(fileName = "CameraVariable", menuName = EditorConst.VariablePrePath + "CameraVariable",
        order = EditorConst.OtherOrder)]
    public class CameraScriptableObject : ScriptableObject
    {
        public IReadOnlyReactiveProperty<float> Height => _height;
        public IReadOnlyReactiveProperty<float> Distance => _distance;
        public IReadOnlyReactiveProperty<Vector3> CenterOffset => _centerOffset;
        
        [SerializeField] private FloatReactiveProperty _height = new(10f);
        [SerializeField] private FloatReactiveProperty _distance = new(15f);
        [SerializeField] private Vector3ReactiveProperty _centerOffset = new(Vector3.zero);
        
    }
}