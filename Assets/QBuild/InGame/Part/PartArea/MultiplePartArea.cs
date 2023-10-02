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
            BlockPartScriptableObject partScriptableObject)
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
            var onThePartPosition = onThePart.transform.position;

            var globalPointEnumerable = onThePart.OnGetConnectPoints().Select(x => x + onThePartPosition);
            var globalPoints = globalPointEnumerable as Vector3[] ?? globalPointEnumerable.ToArray();

            SetPart(_partPlaceAreaN, new Vector3(0, 0, 1), globalPoints, partScriptableObject);
            SetPart(_partPlaceAreaE, new Vector3(1, 0, 0), globalPoints, partScriptableObject);
            SetPart(_partPlaceAreaW, new Vector3(0, 0, -1), globalPoints, partScriptableObject);
            SetPart(_partPlaceAreaS, new Vector3(-1, 0, 0), globalPoints, partScriptableObject);
        }

        private void SetPart(PartPlaceArea part, Vector3 dir, IEnumerable<Vector3> globalPoints,
            BlockPartScriptableObject partScriptableObject)
        {
            var connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(_referencePosition, dir, globalPoints);
            part.SetPart(partScriptableObject, dir, connectPoint);
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
            _partPlaceAreaN.SetKeyIcon(dir);
            _partPlaceAreaE.SetKeyIcon(dir.TurnRight());
            _partPlaceAreaW.SetKeyIcon(dir.TurnRight().TurnRight());
            _partPlaceAreaS.SetKeyIcon(dir.TurnLeft());
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