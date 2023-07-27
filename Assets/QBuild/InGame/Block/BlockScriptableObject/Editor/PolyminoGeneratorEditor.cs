using UnityEditor;
using UnityEngine;

namespace QBuild.BlockScriptableObject
{
    [CustomEditor(typeof(PolyminoGenerator))]
    public class PolyminoGeneratorEditor : Editor
    {
        private PreviewRenderUtility _previewUtility;

        private Mesh _mesh;
        private Material _material;
        private Vector2 _rotation;
        
        private float _distance = 10f;

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public void OnEnable()
        {
            // プレビューの初期設定
            _previewUtility = new PreviewRenderUtility();
            _previewUtility.camera.transform.position = new Vector3(0f, 0f, -10f);

            _previewUtility.camera.nearClipPlane = 5f;
            _previewUtility.camera.farClipPlane = 50f;
            
            // マテリアルの設定
            var prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _material = prim.GetComponent<MeshRenderer>().sharedMaterial;
            DestroyImmediate(prim);

            _mesh = new Mesh();

            // 各ブロックのメッシュ結合して一つにまとめる
            var polyminoGenerator = (PolyminoGenerator)target;
            var generators = polyminoGenerator.GetBlockGenerators();
            var combine = new CombineInstance[generators.Count];
            for (var i = 0; i < generators.Count; i++)
            {
                combine[i].mesh =
                    BlockMesh.GenerateMesh(generators[i].blockGenerator);
                combine[i].transform = Matrix4x4.Translate(generators[i].pos);
            }

            _mesh.CombineMeshes(combine);
        }
        public void OnDisable()
        {
            if (this._previewUtility != null)
            {
                this._previewUtility.Cleanup();
                this._previewUtility = null;
            }
        }
        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (_mesh == null) return;

            _rotation = DragToRotate(r, _rotation);
            _distance = MouseWheel(r, _distance);
            _previewUtility.BeginPreview(r, background);

            _previewUtility.camera.transform.position = Vector3.forward * _distance;
            _previewUtility.camera.transform.LookAt(Vector3.zero);

            _previewUtility.DrawMesh(
                _mesh,
                Matrix4x4.TRS(
                    Vector3.zero,
                    Quaternion.Euler(new Vector3(-_rotation.y, -_rotation.x, 0)),
                    Vector3.one
                ),
                _material, 0
            );

            _previewUtility.camera.Render();

            var resultRender = _previewUtility.EndPreview();
            GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);
        }

        private static Vector2 DragToRotate(Rect r, Vector2 rotation)
        {
            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            var currentEvent = Event.current;

            switch (currentEvent.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                    if (r.Contains(currentEvent.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        currentEvent.Use();
                    }

                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                    }

                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        rotation += currentEvent.delta * 0.5f;
                        currentEvent.Use();
                        GUI.changed = true;
                    }

                    break;
            }

            return rotation;
        }

        private static float MouseWheel(Rect r, float distance)
        {
            if (r.Contains(Event.current.mousePosition) && Event.current.type == EventType.ScrollWheel)
            {
                distance += Event.current.delta.y * 0.1f;
                Event.current.Use();
            }
            return Mathf.Clamp(distance, 1f, 50f);
        }
    }
}