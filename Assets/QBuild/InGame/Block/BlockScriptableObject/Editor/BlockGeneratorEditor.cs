using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QBuild.BlockScriptableObject
{
    [CustomEditor(typeof(BlockGenerator))]
    public class BlockGeneratorEditor : Editor
    {
        private readonly Dictionary<Object, PreviewData> _meshPreviews = new();

        class PreviewData : IDisposable
        {
            bool m_Disposed;

            public readonly PreviewRenderUtility renderUtility;
            public Mesh target { get; private set; }

            public string prefabAssetPath { get; private set; }

            public Bounds renderableBounds { get; private set; }

            public bool useStaticAssetPreview { get; set; }

            public PreviewData(Mesh target)
            {
                renderUtility = new PreviewRenderUtility();
                renderUtility.camera.fieldOfView = 30.0f;
                renderableBounds = new Bounds(new Vector3(0, 0, 0), new Vector3(128, 128));
                this.target = target;
            }


            public void Dispose()
            {
                if (m_Disposed)
                    return;
                renderUtility.Cleanup();
                target = null;
                m_Disposed = true;
            }
        }

        private void OnEnable()
        {
            foreach (var previewTarget in targets)
            {
                if (_meshPreviews.ContainsKey(previewTarget)) continue;

                var blockGenerator = target as BlockGenerator;
                var mesh = UnfoldedBlock.GenerateMesh(blockGenerator);
                var preview = new PreviewData(mesh);
                _meshPreviews.Add(previewTarget, preview);
            }

            PreviewRenderUtility a;
        }

        private void OnDisable()
        {
            foreach (var previewTarget in targets)
            {
                var meshPreview = _meshPreviews[previewTarget];
                meshPreview.Dispose();
            }

            _meshPreviews.Clear();
        }

        private void DoRenderPreview(PreviewData previewData)
        {
            
            previewData.renderUtility.camera.farClipPlane = 100;
            previewData.renderUtility.camera.transform.position = new Vector3(0.5f, 6, -6);
            previewData.renderUtility.camera.transform.rotation = Quaternion.Euler(45, 0, 0);
            previewData.renderUtility.camera.clearFlags = CameraClearFlags.SolidColor;
            
            var prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var material = prim.GetComponent<MeshRenderer>().sharedMaterial;
            previewData.renderUtility.DrawMesh(previewData.target, Matrix4x4.identity, material, 0);
            previewData.renderUtility.Render(true);
            DestroyImmediate(prim);
        }

        public override Texture2D RenderStaticPreview
        (
            string assetPath,
            Object[] subAssets,
            int width,
            int height
        )
        {
            var previewData = _meshPreviews[target];
            previewData.renderUtility.BeginStaticPreview(new Rect(0, 0, width, height));
            DoRenderPreview(previewData);

            return previewData.renderUtility.EndStaticPreview();
        }
    }
}