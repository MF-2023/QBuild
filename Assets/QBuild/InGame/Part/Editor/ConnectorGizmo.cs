using QBuild.Utilities;
using UnityEditor;
using UnityEngine;

namespace QBuild.Part.Editor
{
    public static class ConnectorGizmo
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected, typeof(Connector))]
        private static void DrawGizmo(Connector connector, GizmoType gizmoType)
        {
            Gizmos.color = Color.red;
            foreach (var connectPoint in connector.ConnectMagnet())
            {
                var matrix = connector.transform.localToWorldMatrix;
                var localPosition = matrix.MultiplyPoint(connectPoint.Position);
                Gizmos.DrawSphere(localPosition, 0.025f);

                Gizmos.DrawRay(localPosition, matrix.MultiplyVector(connectPoint.Direction.ToVector3()));
            }
        }
    }
}