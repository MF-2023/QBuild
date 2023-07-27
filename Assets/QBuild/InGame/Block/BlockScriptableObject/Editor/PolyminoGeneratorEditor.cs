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

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public void OnEnable()
        {
            _previewUtility = new PreviewRenderUtility();
            _previewUtility.camera.transform.position = new Vector3(0f, 0f, -10f);

            _previewUtility.camera.nearClipPlane = 5f;
            _previewUtility.camera.farClipPlane = 20f;


            var prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _material = prim.GetComponent<MeshRenderer>().sharedMaterial;
            _mesh = new Mesh();
            DestroyImmediate(prim);

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

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (_mesh == null) return;

            _rotation = DragToRotate(r, _rotation);

            _previewUtility.BeginPreview(r, background);

            _previewUtility.camera.transform.position = Vector3.forward * 10;
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

            // レンダリングされたイメージをGUIに適用します
            Texture resultRender = _previewUtility.EndPreview();
            GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);
        }

        private Vector2 DragToRotate(Rect r, Vector2 rotation)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Event currentEvent = Event.current;

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

        public void OnDisable()
        {
            if (this._previewUtility != null)
            {
                this._previewUtility.Cleanup();
                this._previewUtility = null;
            }
        }
    }
}