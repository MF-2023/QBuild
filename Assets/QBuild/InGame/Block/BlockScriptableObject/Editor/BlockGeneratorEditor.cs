using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QBuild.BlockScriptableObject
{
    [CustomEditor(typeof(BlockGenerator))]
    public class BlockGeneratorEditor : Editor
    {
        private readonly Dictionary<Object, PreviewData> _meshPreviews = new();

        private class PreviewData : IDisposable
        {
            private bool _disposed;

            public readonly PreviewRenderUtility RenderUtility;
            public Mesh Target { get; private set; }

            public PreviewData(Mesh target)
            {
                RenderUtility = new PreviewRenderUtility
                {
                    camera =
                    {
                        fieldOfView = 30.0f
                    }
                };
                this.Target = target;
            }


            public void Dispose()
            {
                if (_disposed)
                    return;
                RenderUtility.Cleanup();
                Target = null;
                _disposed = true;
            }
        }

        private void OnEnable()
        {
            foreach (var previewTarget in targets)
            {
                if (_meshPreviews.ContainsKey(previewTarget)) continue;

                var blockGenerator = target as BlockGenerator;
                if (blockGenerator == null || blockGenerator.GetFaces().Select(x => x.face).Any(face => face == null))
                    return;
                var mesh = UnfoldedBlock.GenerateMesh(blockGenerator);
                var preview = new PreviewData(mesh);
                _meshPreviews.Add(previewTarget, preview);
            }
        }

        private void OnDisable()
        {
            foreach (var previewTarget in targets)
            {
                if (!_meshPreviews.ContainsKey(previewTarget)) continue;
                var meshPreview = _meshPreviews[previewTarget];
                meshPreview.Dispose();
            }

            _meshPreviews.Clear();
        }

        private void DoRenderPreview(PreviewData previewData)
        {
            previewData.RenderUtility.camera.farClipPlane = 100;
            previewData.RenderUtility.camera.transform.position = new Vector3(0.5f, 4, -7);
            previewData.RenderUtility.camera.transform.rotation = Quaternion.Euler(25, 0, 0);
            previewData.RenderUtility.camera.clearFlags = CameraClearFlags.SolidColor;

            var prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var material = prim.GetComponent<MeshRenderer>().sharedMaterial;
            previewData.RenderUtility.DrawMesh(previewData.Target, Matrix4x4.identity, material, 0);
            previewData.RenderUtility.Render(true);
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
            if (!_meshPreviews.ContainsKey(target)) return null;
            var previewData = _meshPreviews[target];
            previewData.RenderUtility.BeginStaticPreview(new Rect(0, 0, width, height));
            DoRenderPreview(previewData);

            return previewData.RenderUtility.EndStaticPreview();
        }
    }
}