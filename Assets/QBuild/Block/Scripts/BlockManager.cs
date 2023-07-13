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

        private List<Block> fallsBlock = new List<Block>();
        [SerializeField] private UnityEvent _onBlockPlacedEvent;
        private float tick = 0;

        private void Awake()
        {
            BlockManagerBind.Init(this);
            generatorCounter = 0;
            _onBlockPlacedEvent.AddListener(this.OnBlockPlaced);
            Block.Init(this);
        }

        private void Start()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    var position = new Vector3Int(x, 0, z);
                    var blockGameObject = Instantiate(_blockPrefab, position, Quaternion.identity,
                        _blocksParent.transform);

                    if (!blockGameObject.TryGetComponent(out Block block)) continue;
                    block.GenerateBlock(_planeBlockGenerator, position);
                    blockGameObject.name = $"plate {position}";
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
            foreach (var block in fallsBlock)
            {
                block.MoveNext();
            }
        }

        private void OnBlockPlaced()
        {
            fallsBlock.Clear();
            StartCoroutine(DelayCreatePolymino());
        }

        private IEnumerator DelayCreatePolymino()
        {
            yield return new WaitForSeconds(1);
            CreatePolymino();
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