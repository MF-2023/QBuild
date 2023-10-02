using System;
using System.Linq;
using UnityEngine;

namespace QBuild.Part
{
    public class MultiplePartArea : MonoBehaviour
    {
        public void UpdatePart(Vector3 onThePartPosition, PartView onThePart,
            BlockPartScriptableObject partScriptableObject)
        {
            var globalPointEnumerable = onThePart.OnGetConnectPoints().Select(x => x + onThePartPosition);
            var globalPoints = globalPointEnumerable as Vector3[] ?? globalPointEnumerable.ToArray();
            var connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(0, 0, 1), globalPoints);
            _partPlaceAreaN.SetPart(partScriptableObject, connectPoint);

            connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(1, 0, 0), globalPoints);
            _partPlaceAreaE.SetPart(partScriptableObject, connectPoint);

            connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(0, 0, -1), globalPoints);
            _partPlaceAreaW.SetPart(partScriptableObject, connectPoint);

            connectPoint =
                PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(-1, 0, 0), globalPoints);
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
        }

        private void Update()
        {
            
        }

        [SerializeField] private PartPlaceArea _partPlaceAreaPrefab;

        private PartPlaceArea _partPlaceAreaN;
        private PartPlaceArea _partPlaceAreaE;
        private PartPlaceArea _partPlaceAreaW;
        private PartPlaceArea _partPlaceAreaS;
    }
}