using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Utilities;
using UnityEngine;

namespace QBuild.Part
{
    public class MultiplePartArea : MonoBehaviour
    {
        public void UpdatePart(Vector3 referencePosition, PartView onThePart,
            BlockPartScriptableObject partScriptableObject, Matrix4x4 multiplePartAreaMatrix)
        {
            if (onThePart == null)
            {
                _partPlaceAreaN.Hide();
                _partPlaceAreaE.Hide();
                _partPlaceAreaW.Hide();
                _partPlaceAreaS.Hide();
                return;
            }

            _referencePosition = referencePosition;

            var globalPointEnumerable =
                onThePart.OnGetConnectPoints().Select(x => onThePart.transform.TransformPoint(x));
            var globalPoints = globalPointEnumerable as Vector3[] ?? globalPointEnumerable.ToArray();


            var connectPointMatrix = Matrix4x4.Rotate(onThePart.transform.rotation);
            var connectPointDirectionV = connectPointMatrix.MultiplyVector(Vector3.forward);

            var dir = DirectionFRBLExtension.VectorToDirectionFRBL(connectPointDirectionV);

            var centerDiff = DirectionUtilities.CalcDirectionFRBL(dir, DirectionFRBL.Forward);

            SetPart(_partPlaceAreaN, Vector3.forward, centerDiff, onThePart, partScriptableObject,
                multiplePartAreaMatrix);
            SetPart(_partPlaceAreaN, Vector3.forward, globalPoints, partScriptableObject, multiplePartAreaMatrix);

            SetPart(_partPlaceAreaE, Vector3.right, centerDiff.TurnRight(), onThePart, partScriptableObject,
                multiplePartAreaMatrix);
            SetPart(_partPlaceAreaE, Vector3.right, globalPoints, partScriptableObject, multiplePartAreaMatrix);

            SetPart(_partPlaceAreaW, Vector3.left, centerDiff.TurnLeft(), onThePart, partScriptableObject,
                multiplePartAreaMatrix);
            SetPart(_partPlaceAreaW, Vector3.left, globalPoints, partScriptableObject, multiplePartAreaMatrix);

            SetPart(_partPlaceAreaS, Vector3.back, centerDiff.TurnRight().TurnRight(), onThePart, partScriptableObject,
                multiplePartAreaMatrix);
            SetPart(_partPlaceAreaS, Vector3.back, globalPoints, partScriptableObject, multiplePartAreaMatrix);
        }

        private void SetPart(PartPlaceArea part, Vector3 dir, IEnumerable<Vector3> globalPoints,
            BlockPartScriptableObject partScriptableObject, Matrix4x4 multiplePartAreaMatrix)
        {
            var connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(_referencePosition, dir, globalPoints);
        }

        private void SetPart(PartPlaceArea part, Vector3 dir, DirectionFRBL dirOnPart, PartView onThePart,
            BlockPartScriptableObject partScriptableObject, Matrix4x4 multiplePartAreaMatrix)
        {
            var targetRotateMatrix = Matrix4x4.Rotate(multiplePartAreaMatrix.rotation).inverse;
            var targetDirectionV = targetRotateMatrix.MultiplyVector(-dir);

            var targetDirection = DirectionFRBLExtension.VectorToDirectionFRBL(targetDirectionV);

            if (partScriptableObject.PartPrefab.HasDirection(targetDirection) &&
                onThePart.TryGetConnectPoint(dirOnPart, out var point))
            {
                part.SetPart(partScriptableObject, dir, onThePart.transform.TransformPoint(point),
                    multiplePartAreaMatrix);
            }
            else
            {
                part.Hide();
            }
        }

        private void Start()
        {
            _partPlaceAreaN = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaN.name += ":N";

            _partPlaceAreaE = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaE.name += ":E";

            _partPlaceAreaW = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaW.name += ":W";

            _partPlaceAreaS = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaS.name += ":S";

            _camera = UnityEngine.Camera.main;
        }

        private void LateUpdate()
        {
            Forward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        }

        private void OnDirChanged()
        {
            var dir = DirectionFRBLExtension.VectorToDirectionFRBL(Forward);
            if (dir == DirectionFRBL.None) return;

            var cameraDiff = DirectionUtilities.CalcDirectionFRBL(dir, DirectionFRBL.Forward);
            var cameraDiff2 = DirectionFRBL.Forward;
            while (dir != DirectionFRBL.Forward)
            {
                dir = dir.TurnRight();
                cameraDiff2 = cameraDiff2.TurnRight();
            }
            
            Debug.Log($"${cameraDiff} ${cameraDiff2}");
            _partPlaceAreaN.SetKeyIcon(cameraDiff);
            _partPlaceAreaE.SetKeyIcon(cameraDiff.TurnRight());
            _partPlaceAreaW.SetKeyIcon(cameraDiff.TurnLeft());
            _partPlaceAreaS.SetKeyIcon(cameraDiff.TurnRight().TurnRight());
        }

        [SerializeField] private PartPlaceArea _partPlaceAreaPrefab;

        private PartPlaceArea _partPlaceAreaN;
        private PartPlaceArea _partPlaceAreaE;
        private PartPlaceArea _partPlaceAreaW;
        private PartPlaceArea _partPlaceAreaS;

        /// <summary>
        /// 向きを出すための基準位置
        /// </summary>
        private Vector3 _referencePosition;

        private UnityEngine.Camera _camera;
        private Vector3 _forward;

        private Vector3 Forward
        {
            get => _forward;
            set
            {
                if (_forward == value) return;
                _forward = value;
                OnDirChanged();
            }
        }
    }
}