using System;
using QBuild.Behavior;
using QBuild.Const;
using QBuild.Stage;
using UnityEngine;
using VContainer;

namespace QBuild.Camera.Center
{
    public class CenterView : MonoBehaviour
    {
        [Inject]
        public void Inject(CameraScriptableObject cameraScriptableObject, StageScriptableObject stageScriptableObject)
        {
            _cameraScriptableObject = cameraScriptableObject;
            _stageScriptableObject = stageScriptableObject;
            
            gameObject.AddComponent<GridMove>();
            _centerPosition = new Vector3(_stageScriptableObject.Width / 2, 0, _stageScriptableObject.Depth / 2);
            _centerPosition -= BlockConst.BlockSizeHalf;
            _centerPosition += _cameraScriptableObject.CenterOffset.Value;
            transform.position = _centerPosition;
        }

        public Vector3 GetCenterPosition()
        {
            return _centerPosition;
        }
        
        private CameraScriptableObject _cameraScriptableObject;
        private StageScriptableObject _stageScriptableObject;
        private Vector3 _centerPosition;
    }
}