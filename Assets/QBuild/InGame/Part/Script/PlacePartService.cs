using System;
using System.Linq;
using UnityEngine;

namespace QBuild.Part
{
    public static class PlacePartService
    {
        public static bool TryPlacePartPosition(BlockPartScriptableObject partScriptableObject, Vector3 connectPoint,
            out Vector3 outPartPosition)
        {
            var func = new Func<Vector3, bool>((pos) =>
            {
                var colliders = partScriptableObject.PartPrefab.GetComponentsInChildren<BoxCollider>();
                foreach (var boxCollider in colliders)
                {
                    if (!Physics.CheckBox(pos + boxCollider.center, (boxCollider.size / 2) * 0.9f,
                            boxCollider.transform.rotation, LayerMask.GetMask("Block"))) continue;
                        return false;
                }

                return true;
            });

            return TryPlacePartPosition(partScriptableObject, connectPoint, func, out outPartPosition);
        }

        public static bool TryPlacePartPosition(BlockPartScriptableObject partScriptableObject, Vector3 connectPoint,
            Func<Vector3, bool> checkCanPlaceFunc,
            out Vector3 outPartPosition)
        {
            outPartPosition = Vector3.zero;
            var newPartConnectPoint = partScriptableObject.PartPrefab.OnGetConnectPoints().ToArray();

            foreach (var point in newPartConnectPoint)
            {
                var newPartPosition = connectPoint - point;
                if (!checkCanPlaceFunc(newPartPosition)) continue;
                outPartPosition = newPartPosition;
                return true;
            }

            return false;
        }
    }
}