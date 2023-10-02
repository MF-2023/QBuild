using QBuild.Starter;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace QBuild.BlockStoreOverlay
{
    [Overlay(typeof(SceneView), MenuPath)]
    public class BlockStoreOverlay : ToolbarOverlay, ITransientOverlay
    {
        public bool visible => EditorApplication.isPlaying && _existBlockStore;

        public override void OnCreated()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                var scopeObject = GameObject.Find("Installer");
                if (scopeObject == null) return;
                if (scopeObject.TryGetComponent(out InGameStater stater))
                {
                    stater.Container.Resolve<BlockStore>();
                }

                _existBlockStore = GameObject.Find("BlockManager").TryGetComponent(out _blockManager);
                OnSelectionChanged();
            }
        }

        private void OnSelectionChanged()
        {
            if (!EditorApplication.isPlaying) return;
            if (Selection.activeGameObject == null) return;

            var target = Selection.activeGameObject;
            if (target == null) return;
            if (!target.TryGetComponent(out Block block))
            {
                if(Selection.activeGameObject.transform.parent == null) return;
                target = Selection.activeGameObject.transform.parent.gameObject;
                if (!target.TryGetComponent(out block))
                {
                    return;
                }
            }
            _blockStoreView.BindBlock(block);
            _blockStoreView.SetPosition(block.GetGridPosition());
        }

        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();
            _blockStoreView = new BlockStoreView();
            _blockStoreView.OnBlockSelected += OnSelectPositionChanged;
            _blockStoreView.OnChangedBlockType += type =>
            {
                if (type == null) return;
                var block = _blockStoreView.GetBlock();
                if (block != null)
                {
                    block.GenerateFaces(type);
                }
                else
                {
                    _blockManager.TryGenerateBlock(type, _selectedPosition, out block);
                    Selection.activeGameObject = block.gameObject;
                    _blockStoreView.BindBlock(block);
                }
            };
            root.Add(_blockStoreView);
            return root;
        }


        private void OnSelectPositionChanged(Vector3Int position)
        {
            _selectedPosition = position;

            if (TryBindBlock(position))
            {
                Selection.activeGameObject = _blockStoreView.GetBlock().gameObject;
            }
            else
            {
                _blockStoreView.UnbindBlock();
            }
        }

        private bool TryBindBlock(Vector3Int position)
        {
            if (_blockManager == null) return false;
            if (!_blockManager.TryGetBlock(position, out var block))
            {
                return false;
            }

            _blockStoreView.BindBlock(block);
            return true;
        }

        private const string MenuPath = "Custom/SceneControl";
        private bool _existBlockStore = false;
        private BlockStoreView _blockStoreView;
        private BlockManager _blockManager;
        private Vector3Int _selectedPosition;
    }
}