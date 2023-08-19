using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QBuild.Condition;
using QBuild.Mino;
using QBuild.Stage;
using SherbetInspector.Core.Attributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VContainer;

namespace QBuild
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private List<MinoType> _polyminoGenerators;
        [FormerlySerializedAs("_polyminoGeneratorList")] [SerializeField] private MinoTypeList _minoTypeList;

        [SerializeField] private Vector3Int _blockSpawnPosition;

        [FormerlySerializedAs("_planeBlockGenerator")] [SerializeField] private BlockType _planeBlockType;
        [SerializeField] private GameObject _floorParent;

        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _blocksParent;
        private int _generatorCounter = 0;

        [SerializeField] private List<Block> _blocks;

        [SerializeField] private List<Polyomino> _polyominos;
        private readonly Dictionary<long, Polyomino> _polyominoDictionary = new();

        [SerializeField] private List<Block> _blockTable;

        [SerializeField] private List<Polyomino> _fallsMino = new List<Polyomino>();
        [SerializeField] private UnityEvent _onBlockPlacedEvent;
        private float _tick = 0;

        [SerializeField] private Vector3Int _maxArea;

        [SerializeField] private GameObject _plannedSitePrefab;
        private readonly List<GameObject> _plannedSites = new();
        [SerializeField] private StabilityCalculator stabilityCalculator = new StabilityCalculator();

        [SerializeField] private FaceJointMatrix _conditionMap;


        [SerializeField] private List<int> _ratio = new();

        private void Awake()
        {
            BlockManagerBind.Init(this);
            stabilityCalculator.Init(this);
            _generatorCounter = 0;
            _onBlockPlacedEvent.AddListener(this.OnBlockPlaced);

            Block.Init(this);
            Polyomino.Init(this);

            var capacity = _maxArea.x * _maxArea.y * _maxArea.z;

            _blockTable = new List<Block>(capacity);
            for (var i = 0; i < capacity; i++) _blockTable.Add(null);
        }


        private void Start()
        {
            var floorBlocks = _stageFactory.CreateFloor(_blockPrefab, _floorParent);
            _blocks.AddRange(floorBlocks);
        }

        [Inject]
        private void Inject(StageFactory factory)
        {
            Debug.Log("Inject");
            _stageFactory = factory;
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
            _tick += Time.deltaTime;

            var key = Keyboard.current;
            if (key.spaceKey.isPressed)
            {
                _tick += Time.deltaTime * 2;
            }

            if (_tick < 0.4f) return;
            _tick = 0;

            FallMinoUpdate(new Vector3Int(0, -1, 0));
        }


        private void FallMinoUpdate(Vector3Int move)
        {
            var stoppedBlocks = new List<Polyomino>();
            foreach (var mino in _fallsMino)
            {
                mino.MoveNext(move);
                if (!mino.IsFalling)
                {
                    stoppedBlocks.Add(mino);
                }
            }

            if (_fallsMino.Count != 0 && stoppedBlocks.Count == _fallsMino.Count)
            {
                _onBlockPlacedEvent.Invoke();
            }
            else
            {
                ProvisionalBlockUpdate();
            }
        }

        private void ProvisionalBlockUpdate()
        {
            foreach (var obj in _plannedSites)
            {
                Destroy(obj);
            }

            _plannedSites.Clear();

            foreach (var mino in _fallsMino)
            {
                var positions = mino.GetProvisionalPlacePosition();

                foreach (var plannedSite in positions.Select(pos =>
                             Instantiate(_plannedSitePrefab, pos, Quaternion.identity, _blocksParent.transform)))
                {
                    _plannedSites.Add(plannedSite);
                }
            }
        }

        private void OnBlockPlaced()
        {
            var removeBlocks = new HashSet<Vector3Int>();
            foreach (var block in _fallsMino[0].GetBlocks())
            {
                var list = stabilityCalculator.CalcPhysicsStabilityToFall(block.GetGridPosition(), 32,
                    out var stability);

                if (!list.Any()) continue;
                Debug.Log($"list:{list.Count} stability:{stability}");
                foreach (var pos in list)
                {
                    Debug.Log($"pos:{pos}");
                    removeBlocks.Add(pos);
                }
            }

            foreach (var removeBlockPosition in removeBlocks)
            {
                TryGetBlock(removeBlockPosition, out var fallBlock);
                RemoveBlock(fallBlock);
            }

            _fallsMino.Clear();
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

        public bool TryGetMino(long key, out Polyomino mino)
        {
            mino = null;
            if (!_polyominoDictionary.ContainsKey(key)) return false;

            mino = _polyominoDictionary[key];
            return true;
        }

        public bool RemoveMino(long key)
        {
            return _polyominoDictionary.Remove(key);
        }

        private IEnumerator DelayCreatePolymino()
        {
            yield return new WaitForSeconds(0.5f);
            CreatePolyomino();
        }


        public bool ContactCondition(Block owner, Block other)
        {
            var dir = other.GetGridPosition() - owner.GetGridPosition();
            var faceDirType = dir.ToVectorBlockFace();
            var ownerFace = owner.GetFace(faceDirType);
            var otherFace = other.GetFace(faceDirType.Opposite());

            return _conditionMap.GetCondition(ownerFace.GetFaceType(), otherFace.GetFaceType());
        }

        public bool ContactTest(Block owner, Block other)
        {
            var dir = other.GetGridPosition() - owner.GetGridPosition();
            var faceDirType = dir.ToVectorBlockFace();
            var ownerFace = owner.GetFace(faceDirType);
            var otherFace = other.GetFace(faceDirType.Opposite());

            Debug.Log(ownerFace.GetFaceType().name);
            return (ownerFace.GetFaceType().name == "Convex" && otherFace.GetFaceType().name == "Concave") ||
                   (ownerFace.GetFaceType().name == "Concave" && otherFace.GetFaceType().name == "Convex");
        }

        private int CalcVector3ToIndex(Vector3Int v)
        {
            var result = v.x + v.z * _maxArea.x + v.y * _maxArea.x * _maxArea.z;
            return result;
        }

        public void OnStartGame()
        {
            CreatePolyomino();
        }

        List<Color> ColorTable = new List<Color>()
        {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,
            Color.yellow,
        };

        [Button]
        private void CreatePolyomino()
        {
            if (_minoTypeList == null)
            {
                Debug.LogError("_polyminoGeneratorList:生成リストが登録されていません。", this);
            }

            var generators = _minoTypeList.NextGenerator();
            if (_generatorCounter >= _minoTypeList.GetCount())
            {
                _generatorCounter = 0;
            }

            var polyomino = new Polyomino();

            var polyominoSize = _polyominoDictionary.Count;

            _polyominoDictionary.Add(polyominoSize, polyomino);
            polyomino.SetDictionaryKey(polyominoSize);
            var color = ColorTable[_polyominos.Count % ColorTable.Count];
            //カラーテーブルを周回するごとに色を明るくする

            var t = (_polyominos.Count / ColorTable.Count) / 5f;
            color = Color.Lerp(color, Color.white, t);
            foreach (var positionToBlockGenerator in generators.GetBlockTypes())
            {
                var position = positionToBlockGenerator._pos + _blockSpawnPosition;
                var blockGameObject = Instantiate(_blockPrefab, position, Quaternion.identity, _blocksParent.transform);

                if (!blockGameObject.TryGetComponent(out Block block)) continue;
                block.GenerateBlock(positionToBlockGenerator._blockType, position, polyomino.GetDictionaryKey());


                foreach (var renderer in block.GetComponentsInChildren<Renderer>())
                {
                    renderer.material.color = color;
                }

                blockGameObject.name = $"Block {position}";
                polyomino.AddBlock(block);
                _blocks.Add(block);
            }

            _fallsMino.Add(polyomino);
            _polyominos.Add(polyomino);

            _generatorCounter++;
            Debug.Log("Created Polymino");
        }


        public void UpdateBlock(Block block)
        {
            var index = CalcVector3ToIndex(block.GetGridPosition());
            Debug.Log($"UpdateBlock Pos:{block.GetGridPosition()} index:{index}");
            _blockTable[index] = block;
        }

        public void UpdateBlock(Block block, Vector3Int beforePosition)
        {
            if (_blockTable[CalcVector3ToIndex(beforePosition)] == block)
                _blockTable[CalcVector3ToIndex(beforePosition)] = null;
            Debug.Log(
                $"UpdateBlock Pos:{block.GetGridPosition()} before:{beforePosition} index:{CalcVector3ToIndex(block.GetGridPosition())}");
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

        private StageFactory _stageFactory;


#if UNITY_EDITOR

        [Button]
        private void CreateStart()
        {
            if (!EditorApplication.isPlaying) return;
            CreatePolyomino();
        }

#endif
    }
}