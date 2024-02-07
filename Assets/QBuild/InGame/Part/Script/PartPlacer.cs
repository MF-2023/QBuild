using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Part.PartScriptableObject;
using QBuild.Part.Presenter;
using QBuild.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using VContainer;

namespace QBuild.Part
{
    public class PartPlacer : MonoBehaviour
    {
        public event Action<PartView> OnPlaceEvent = delegate { };
        [SerializeField] private UnityEvent<PartView> _thePartHasChanged;

        [Inject]
        private void Inject(@InputSystem inputSystem, HolderPresenter holderPresenter, PartRepository repository,
            BasePartSpawnConfiguratorObject partListScriptableObject)
        {
            _camera = Camera.main;

            inputSystem.InGame.BlockPlaceF.performed += _ => ForwardPlacePart();
            inputSystem.InGame.BlockPlaceR.performed += _ => RightPlacePart();
            inputSystem.InGame.BlockPlaceB.performed += _ => BackPlacePart();
            inputSystem.InGame.BlockPlaceL.performed += _ => LeftPlacePart();

            inputSystem.InGame.SelectChange.performed += ChangeSelect;
            inputSystem.InGame.BlockRotation.performed += BlockRotation;

            _partListScriptableObject = partListScriptableObject;
            _nextPartHolder =
                new PlayerPartHolder(_partListScriptableObject, _partListScriptableObject.GetPartObjectCount);
            _nextPartHolder.OnChangedSelect += OnSelectChanged;
            holderPresenter.Bind(_nextPartHolder);
            _nextPartHolder.Initialize();
            OnPlaceEvent += repository.AddPart;
        }

        private void ChangeSelect(InputAction.CallbackContext context)
        {
            var value = (int)context.ReadValue<float>();
            if (value == 0) return;
            if (value > 0)
            {
                _nextPartHolder.Next();
            }
            else
            {
                _nextPartHolder.Prev();
            }
        }

        private void Update()
        {
            OnThePartUpdate();

            /*
            if (Keyboard.current[Key.R].wasPressedThisFrame)
                CurrentRotateIndex = (CurrentRotateIndex + 1) % RotateMap.Count;
            if (Keyboard.current[Key.T].wasPressedThisFrame)
                CurrentRotateIndex = (CurrentRotateIndex - 1 + RotateMap.Count) % RotateMap.Count;
            */
        }

        private void BlockRotation(InputAction.CallbackContext context)
        {
            var value = (int)context.ReadValue<float>();
            if (value == 0) return;
            if (value > 0)
            {
                CurrentRotateIndex = (CurrentRotateIndex + 1) % RotateMap.Count;
            }
            else
            {
                CurrentRotateIndex = (CurrentRotateIndex - 1 + RotateMap.Count) % RotateMap.Count;
            }
        }

        private void ForwardPlacePart()
        {
            DirPlacePart(Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized);
        }

        private void RightPlacePart()
        {
            DirPlacePart(_camera.transform.right);
        }

        private void BackPlacePart()
        {
            DirPlacePart(_camera.transform.forward * -1);
        }

        private void LeftPlacePart()
        {
            DirPlacePart(_camera.transform.right * -1);
        }

        private void DirPlacePart(Vector3 dir)
        {
            if (CurrentOnThePart == null) return;
            var connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, dir,
                CurrentOnThePart.OnGetConnectPoints().Select(x => CurrentOnThePart.transform.TransformPoint(x)));
            Place(dir, connectPoint, CurrentRotateMatrix());
        }

        private void Place(Vector3 dir, Vector3 connectPoint, Matrix4x4 multiplePartAreaMatrix)
        {
            // 次の設置するパーツを取得
            var partScriptableObject = CurrentPart();

            var tryPlaceInfo = new TryPlaceInfo(partScriptableObject, dir, connectPoint, multiplePartAreaMatrix);
            if (PlacePartService.TryPlacePartPosition(tryPlaceInfo, out var outMatrix))
            {
                _nextPartHolder.Use();

                var pos = outMatrix.GetPosition();
                pos.x = (float)Math.Round(pos.x, 4);
                pos.y = (float)Math.Round(pos.y, 4);
                pos.z = (float)Math.Round(pos.z, 4);
                var view = Instantiate(partScriptableObject.PartPrefab, pos, outMatrix.rotation);
                Instantiate(_particlePrefab, view.transform);
                view.Direction =
                    DirectionFRBLExtension.VectorToDirectionFRBL(
                        multiplePartAreaMatrix.MultiplyVector(Vector3.forward));


                PartConnectService.ConnectPart(CurrentOnThePart, DirectionFRBLExtension.VectorToDirectionFRBL(dir),
                    view);

                OnPlaceEvent?.Invoke(view);
                OnThePartChanged();
            }
        }

        private void OnThePartUpdate()
        {
            var hit = new RaycastHit[1];
            var size = Physics.RaycastNonAlloc(transform.position + Vector3.up * 0.5f, Vector3.down, hit, 1.5f,
                LayerMask.GetMask("Block"));
            if (size > 0)
            {
                CurrentOnThePart = hit[0].collider.GetComponentInParent<PartView>();
            }
            else
            {
                CurrentOnThePart = null;
            }
        }

        private void OnThePartChanged()
        {
            if (_currentOnThePart != null) _thePartHasChanged?.Invoke(_currentOnThePart);
            _multiplePartArea.UpdatePart(transform.position, _currentOnThePart, CurrentPart(), CurrentRotateMatrix());
        }

        private void OnSelectChanged(object o, HolderSelectChangeEventArgs e)
        {
            OnThePartChanged();
        }

        private BlockPartScriptableObject CurrentPart()
        {
            return _nextPartHolder.GetCurrentPart();
        }

        public void OnReset()
        {
            OnThePartUpdate();
        }

        [SerializeField] private BasePartSpawnConfiguratorObject _partListScriptableObject;

        private PlayerPartHolder _nextPartHolder;

        [SerializeField] private PartView _currentOnThePart;
        [SerializeField] private MultiplePartArea _multiplePartArea;

        [SerializeField] private GameObject _particlePrefab;

        private PartView CurrentOnThePart
        {
            get => _currentOnThePart;
            set
            {
                if (_currentOnThePart == value) return;
                _currentOnThePart = value;
                OnThePartChanged();
            }
        }

        private int _currentRotateIndex = 0;

        public int CurrentRotateIndex
        {
            get { return _currentRotateIndex; }
            set
            {
                _currentRotateIndex = value;
                //TODO 回転時用のメソッドを別途準備する
                OnThePartChanged();
            }
        }

        private Matrix4x4 CurrentRotateMatrix()
        {
            return Matrix4x4.Rotate(RotateMap[_currentRotateIndex]);
        }

        private static readonly List<Quaternion> RotateMap = new List<Quaternion>()
        {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 180, 0),
            Quaternion.Euler(0, -90, 0),
        };

        private Camera _camera;
    }
}