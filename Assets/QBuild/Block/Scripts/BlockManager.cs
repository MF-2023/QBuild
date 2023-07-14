using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SherbetInspector.Core.Attributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace QBuild
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private List<PolyminoGenerator> _polyminoGenerators;
        [SerializeField] private PolyminoGeneratorList _polyminoGeneratorList;

        [SerializeField] private Vector3Int _blockSpawnPosition;

        [SerializeField] private BlockGenerator _planeBlockGenerator;
        [SerializeField] private GameObject _floorParent;

        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _blocksParent;
        private int generatorCounter = 0;

        [SerializeField] private List<Block> _blocks;

        [SerializeField] private List<Polyomino> _polyominos;

        private List<Block> _blockTable;

        private List<Block> fallsBlock = new List<Block>();
        [SerializeField] private UnityEvent _onBlockPlacedEvent;
        private float tick = 0;

        [SerializeField] private Vector3Int _maxArea;

        private void Awake()
        {
            BlockManagerBind.Init(this);
            generatorCounter = 0;
            _onBlockPlacedEvent.AddListener(this.OnBlockPlaced);
            Block.Init(this);
            var capacity = _maxArea.x * _maxArea.y * _maxArea.z;

            _blockTable = new List<Block>(capacity);
            for (int i = 0; i < capacity; i++) _blockTable.Add(null);
        }


        private void Start()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    var position = new Vector3Int(x, 0, z);
                    var blockGameObject = Instantiate(_blockPrefab, position, Quaternion.identity,
                        _floorParent.transform);

                    if (!blockGameObject.TryGetComponent(out Block block)) continue;
                    block.GenerateBlock(_planeBlockGenerator, position);
                    block.OnBlockPlaced();
                    blockGameObject.name = $"floor {position}";
                    _blocks.Add(block);
                }
            }
        }

        private void Update()
        {
            TickUpdate();
        }

        private void TickUpdate()
        {
            tick += Time.deltaTime;

            if (tick < 1) return;
            tick = 0;
            var dirs = new Vector3Int[]
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, -1, 0)
            };
            var stoppedBlocks = new List<Block>();
            foreach (var block in fallsBlock)
            {
                block.MoveNext();

                foreach (var pos in dirs.Select(x => x + block.GetGridPosition()))
                {
                    if (!TryGetBlock(pos, out var dirBlock)) continue;
                    if (!dirBlock.IsFalling())
                    {
                        stoppedBlocks.Add(block);
                    }
                }
            }

            foreach (var block in stoppedBlocks)
            {
                block.OnBlockPlaced();
                fallsBlock.Remove(block);
            }

            if (stoppedBlocks.Count > 0 && fallsBlock.Count == 0)
            {
                OnBlockPlaced();
            }
        }

        private void OnBlockPlaced()
        {
            fallsBlock.Clear();
            StartCoroutine(DelayCreatePolymino());
        }

        private bool TryGetBlock(Vector3Int position, out Block block)
        {
            var index = CalcVector3ToIndex(position);
            if (index < 0 || index >= _blockTable.Count)
            {
                block = null;
                return false;
            }

            block = _blockTable[index];
            return block != null;
        }

        private IEnumerator DelayCreatePolymino()
        {
            yield return new WaitForSeconds(1);
            CreatePolymino();
        }

        private int CalcVector3ToIndex(Vector3Int v)
        {
            var result = v.x + v.z * _maxArea.x + v.y * _maxArea.x * _maxArea.z;
            return result;
        }

#if UNITY_EDITOR

        [Button]
        private void CreateStart()
        {
            if (!EditorApplication.isPlaying) return;
            CreatePolymino();
        }

#endif
        [Button]
        private void CreatePolymino()
        {
            if (_polyminoGeneratorList == null)
            {
                Debug.LogError("_polyminoGeneratorList:生成リストが登録されていません。", this);
            }

            var generators = _polyminoGeneratorList.Generators();
            if (generatorCounter >= generators.Count)
            {
                generatorCounter = 0;
            }

            var polyomino = new Polyomino();
            foreach (var positionToBlockGenerator in generators[generatorCounter].GetBlockGenerators())
            {
                var position = positionToBlockGenerator.pos + _blockSpawnPosition;
                var blockGameObject = Instantiate(_blockPrefab, position, Quaternion.identity, _blocksParent.transform);

                if (!blockGameObject.TryGetComponent(out Block block)) continue;
                block.GenerateBlock(positionToBlockGenerator.blockGenerator, position);
                blockGameObject.name = $"Block {position}";
                polyomino.AddBlock(block);
                _blocks.Add(block);
                fallsBlock.Add(block);
            }

            _polyominos.Add(polyomino);

            generatorCounter++;
            Debug.Log("Created Polymino");
        }

        public void UpdateBlock(Block block)
        {
            _blockTable[CalcVector3ToIndex(block.GetGridPosition())] = block;
        }

        public void UpdateBlock(Block block, Vector3Int beforePosition)
        {
            _blockTable[CalcVector3ToIndex(beforePosition)] = null;
            _blockTable[CalcVector3ToIndex(block.GetGridPosition())] = block;
        }

        [Button]
        public void Clear()
        {
            _polyominos.Clear();
            _blocks.Clear();
            foreach (var child in _blocksParent.transform.OfType<Transform>().ToArray())
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}