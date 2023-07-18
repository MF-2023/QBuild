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
using UnityEngine.InputSystem;

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

        [SerializeField] private List<Polyomino> fallsMino = new List<Polyomino>();
        [SerializeField] private UnityEvent _onBlockPlacedEvent;
        private float tick = 0;

        [SerializeField] private Vector3Int _maxArea;

        [SerializeField] private GameObject _plannedSitePrefab;
        private List<GameObject> plannedSites = new List<GameObject>();
        [SerializeField] private StabilityCalculator stabilityCalculator = new StabilityCalculator();
        private void Awake()
        {
            BlockManagerBind.Init(this);
            stabilityCalculator.Init(this);
            generatorCounter = 0;
            _onBlockPlacedEvent.AddListener(this.OnBlockPlaced);

            Block.Init(this);
            Polyomino.Init(this);

            var capacity = _maxArea.x * _maxArea.y * _maxArea.z;

            _blockTable = new List<Block>(capacity);
            for (int i = 0; i < capacity; i++) _blockTable.Add(null);
        }


        private void Start()
        {
            for (var x = 0; x < 10; x++)
            {
                for (var z = 0; z < 10; z++)
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
            InputUpdate();
            TickUpdate();
        }

        private void InputUpdate()
        {
            var key = Keyboard.current;
            Vector3Int move = Vector3Int.zero;
            if (key.aKey.wasPressedThisFrame)
            {
                move += new Vector3Int(-1, 0, 0);
            }

            if (key.sKey.wasPressedThisFrame)
            {
                move += new Vector3Int(0, 0, -1);
            }

            if (key.dKey.wasPressedThisFrame)
            {
                move += new Vector3Int(1, 0, 0);
            }

            if (key.wKey.wasPressedThisFrame)
            {
                move += new Vector3Int(0, 0, 1);
            }

            if (move == Vector3Int.zero) return;

            FallMinoUpdate(move);
        }

        private void TickUpdate()
        {
            tick += Time.deltaTime;

            if (tick < 0.8f) return;
            tick = 0;

            FallMinoUpdate(new Vector3Int(0, -1, 0));
        }


        private void FallMinoUpdate(Vector3Int move)
        {
            var stoppedBlocks = new List<Polyomino>();
            foreach (var mino in fallsMino)
            {
                mino.MoveNext(move);
                if (!mino.isFalling)
                {
                    stoppedBlocks.Add(mino);
                }
            }

            if (fallsMino.Count != 0 && stoppedBlocks.Count == fallsMino.Count)
            {
                OnBlockPlaced();
            }
            else
            {
                ProvisionalBlockUpdate();
            }
        }

        private void ProvisionalBlockUpdate()
        {
            foreach (var obj in plannedSites)
            {
                Destroy(obj);
            }
            plannedSites.Clear();

            foreach (var mino in fallsMino)
            {
                var positions = mino.GetProvisionalPlacePosition();

                foreach (var plannedSite in positions.Select(pos => Instantiate(_plannedSitePrefab, pos, Quaternion.identity, _blocksParent.transform)))
                {
                    plannedSites.Add(plannedSite);
                }
            }
        }

        private void OnBlockPlaced()
        {
            foreach (var block in fallsMino[0].GetBlocks())
            {
                var list = stabilityCalculator.CalcPhysicsStabilityToFall(block.GetGridPosition(),32,out var stability);

                if (list.Any())
                {
                    Debug.Log($"list:{list.Count} stability:{stability}");
                    foreach (var pos in list)
                    {
                        Debug.Log($"pos:{pos}");
                        TryGetBlock(pos, out var fallBlock);
                        RemoveBlock(fallBlock);
                    }
                }
            }
            fallsMino.Clear();
            StartCoroutine(DelayCreatePolymino());
        }

        public bool CanPlace(Vector3Int position)
        {
            if (position.x >= _maxArea.x || position.x < 0) return false;
            if (position.y >= _maxArea.y || position.y < 0) return false;
            if (position.z >= _maxArea.z || position.z < 0) return false;

            return true;
        }

        public bool TryGetBlock(Vector3Int position, out Block block)
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
            yield return new WaitForSeconds(0.5f);
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
            }

            fallsMino.Add(polyomino);
            _polyominos.Add(polyomino);

            generatorCounter++;
            Debug.Log("Created Polymino");
        }

        public void UpdateBlock(Block block)
        {
            var index = CalcVector3ToIndex(block.GetGridPosition());
            _blockTable[index] = block;
            
        }

        public void UpdateBlock(Block block, Vector3Int beforePosition)
        {
            _blockTable[CalcVector3ToIndex(beforePosition)] = null;
            _blockTable[CalcVector3ToIndex(block.GetGridPosition())] = block;
        }

        public void RemoveBlock(Block block)
        {
            _blockTable[CalcVector3ToIndex(block.GetGridPosition())] = null;
            _blocks.Remove(block);
            Destroy(block.gameObject);
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