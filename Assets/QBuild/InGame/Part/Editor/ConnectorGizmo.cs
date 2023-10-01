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
            foreach (var connectPoint in connector.ConnectPoints)
            {
                Gizmos.DrawSphere(connector.transform.position + connectPoint, 0.025f);
            }
        }
    }
}