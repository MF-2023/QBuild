using System;
using UniRx;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace QBuild.BlockStoreOverlay
{
    public class BlockStoreView : VisualElement
    {
        public event Action<Vector3Int> OnBlockSelected;
        public event Action<BlockType> OnChangedBlockType;

        public BlockStoreView()
        {
            var tree = UIToolkitUtility.GetVisualTree("BlockStoreOverlay/BlockStoreView");
            var baseElement = tree.Instantiate();
            Add(baseElement);
            Initialize();
        }

        private void Initialize()
        {
            var element = this.Q<Vector3IntField>();
            element.RegisterCallback<ChangeEvent<Vector3Int>>(w => { OnBlockSelected?.Invoke(w.newValue); });
            _blockTypeField = this.Q<ObjectField>();
        }

        private void BlockTypeChanged(ChangeEvent<Object> w)
        {
            if (w.previousValue == w.newValue) return;
            OnChangedBlockType?.Invoke((BlockType)w.newValue);
        }

        public void BindBlock(Block block)
        {
            UnbindBlock();
            _block = block;
            this.Bind(new SerializedObject(_block));
            _blockTypeField.RegisterValueChangedCallback(BlockTypeChanged);
        }

        public void UnbindBlock()
        {
            this.Unbind();

            _block = null;
            _blockTypeField.value = null;
            _blockTypeField.UnregisterValueChangedCallback(BlockTypeChanged);
        }

        public Block GetBlock()
        {
            return _block;
        }
        public void SetPosition(Vector3Int position)
        {
            this.Q<Vector3IntField>().value = position;
        }

        private Block _block;

        private ObjectField _blockTypeField;
    }
}