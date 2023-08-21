﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QBuild.Mino;
using QBuild.Stage;
using SherbetInspector.Core.Attributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using VContainer;

namespace QBuild
{
    public class BlockManager : MonoBehaviour
    {
        private void Awake()
        {
            BlockManagerBind.Init(this);
            _onBlockPlacedEvent.AddListener(this.OnBlockPlaced);
        }

        private void Start()
        {
            _stageFactory.CreateFloor(_floorParent);
        }

        [Inject]
        private void Inject(StageFactory factory, IMinoFactory minoFactory, BlockService blockService, MinoService minoService,StabilityCalculator stabilityCalculator)
        {
            Debug.Log("Inject BlockManager");
            _stageFactory = factory;
            _minoFactory = minoFactory;
            _blockService = blockService;
            _minoService = minoService;
            _stabilityCalculator = stabilityCalculator;
        }

        private void Update()
        {
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
            if (_fallsMino == null) return;
            var stoppedBlocks = new List<Polyomino>();
            _minoService.TranslationMino(_fallsMino, move);
            if (!_fallsMino.IsFalling)
            {
                stoppedBlocks.Add(_fallsMino);
            }

            if (_fallsMino != null && stoppedBlocks.Count > 0)
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

            var positions = _minoService.GetProvisionalPlacePosition(_fallsMino);

            foreach (var plannedSite in positions.Select(pos =>
                         Instantiate(_plannedSitePrefab, pos, Quaternion.identity, _blocksParent.transform)))
            {
                _plannedSites.Add(plannedSite);
            }
        }

        private void OnBlockPlaced()
        {
            var removeBlocks = new HashSet<Vector3Int>();
            foreach (var block in _fallsMino.GetBlocks())
            {
                var list = _stabilityCalculator.CalcPhysicsStabilityToFall(block.GetGridPosition(), 32,
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
                _blockService.TryGetBlock(removeBlockPosition, out var fallBlock);
                _blockService.RemoveBlock(fallBlock);
            }

            _fallsMino = null;
            StartCoroutine(DelayCreateMino());
        }

        private IEnumerator DelayCreateMino()
        {
            yield return new WaitForSeconds(0.5f);
            CreatePolyomino();
        }

        public void OnStartGame()
        {
            CreatePolyomino();
        }


        [Button]
        private void CreatePolyomino()
        {
            if (_minoTypeList == null)
            {
                Debug.LogError("_minoGeneratorList:生成リストが登録されていません。", this);
            }

            if (_fallsMino != null)
            {
                Debug.LogError("既にミノが落下中です。", this);
            }

            var minoType = _minoTypeList.NextGenerator();
            _fallsMino = _minoFactory.CreateMino(minoType, _blockSpawnPosition, _blocksParent.transform);
        }

        [Button]
        public void Clear()
        {
            _blockService.Clear();
            _minoService.Clear();
            foreach (var child in _blocksParent.transform.OfType<Transform>().ToArray())
            {
                DestroyImmediate(child.gameObject);
            }
        }

        [SerializeField] private List<MinoType> _minoGenerators;

        [SerializeField]
        private MinoTypeList _minoTypeList;

        [SerializeField] private Vector3Int _blockSpawnPosition;

        [SerializeField] private GameObject _floorParent;

        [SerializeField] private GameObject _blocksParent;


        [SerializeField] private UnityEvent _onBlockPlacedEvent;

        [SerializeField] private GameObject _plannedSitePrefab;

        private readonly List<GameObject> _plannedSites = new();
        private float _tick = 0;
        private Polyomino _fallsMino;

        private StageFactory _stageFactory;

        private BlockService _blockService;
        private MinoService _minoService;
        private IMinoFactory _minoFactory;

        private StabilityCalculator _stabilityCalculator;

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