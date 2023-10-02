﻿using System;
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
            _referencePosition = referencePosition;
            var onThePartPosition = onThePart.transform.position;

            var globalPointEnumerable = onThePart.OnGetConnectPoints().Select(x => x + onThePartPosition);
            var globalPoints = globalPointEnumerable as Vector3[] ?? globalPointEnumerable.ToArray();
            var connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(_referencePosition, new Vector3(0, 0, 1), globalPoints);
            _partPlaceAreaN.SetPart(partScriptableObject, connectPoint);

            connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(_referencePosition, new Vector3(1, 0, 0), globalPoints);
            _partPlaceAreaE.SetPart(partScriptableObject, connectPoint);

            connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(_referencePosition, new Vector3(0, 0, -1), globalPoints);
            _partPlaceAreaW.SetPart(partScriptableObject, connectPoint);

            connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(_referencePosition, new Vector3(-1, 0, 0), globalPoints);
            _partPlaceAreaS.SetPart(partScriptableObject, connectPoint);
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
            if(dir == DirectionFRBL.None) return;
            _partPlaceAreaN.SetKeyIcon(dir.TurnRight().TurnRight());
            _partPlaceAreaE.SetKeyIcon(dir.TurnLeft());
            _partPlaceAreaW.SetKeyIcon(dir);
            _partPlaceAreaS.SetKeyIcon(dir.TurnRight());
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
                if(_forward == value) return;
                _forward = value;
                OnDirChanged();
            }
        }
    }
}