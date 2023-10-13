using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Part.Presenter;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace QBuild.Part
{
    public class ChangeSelectEvent
    {
        public int Index { get; set; }
        public int PrevIndex { get; set; }

        public ChangeSelectEvent(int index, int prevIndex)
        {
            Index = index;
            PrevIndex = prevIndex;
        }
    }

    public class PartPlacer : MonoBehaviour
    {
        public event Action<ChangeSelectEvent> OnSelectChangedEvent = delegate { };

        [Inject]
        private void Inject(@InputSystem inputSystem, HolderPresenter holderPresenter)
        {
            inputSystem.InGame.BlockPlaceF.performed += _ => ForwardPlacePart();
            inputSystem.InGame.BlockPlaceR.performed += _ => RightPlacePart();
            inputSystem.InGame.BlockPlaceB.performed += _ => BackPlacePart();
            inputSystem.InGame.BlockPlaceL.performed += _ => LeftPlacePart();

            inputSystem.InGame.SelectChange.performed += ChangeSelect;

            _nextPartHolders.Add(new NextPartHolder(_partListScriptableObject));
            _nextPartHolders.Add(new NextPartHolder(_partListScriptableObject));
            _nextPartHolders.Add(new NextPartHolder(_partListScriptableObject));

            _currentSelectHolderIndex = 1;
            holderPresenter.Bind(this);
        }

        private void ChangeSelect(InputAction.CallbackContext context)
        {
            var value = (int) context.ReadValue<float>();
            if (value == 0) return;
            var nextIndex = _currentSelectHolderIndex + value;
            if (nextIndex < 0) nextIndex = _nextPartHolders.Count - 1;
            if (nextIndex >= _nextPartHolders.Count) nextIndex = 0;
            CurrentSelectHolderIndex = nextIndex;
        }

        private void Start()
        {
        }

        private void Update()
        {
            OnThePartUpdate();


            if(Keyboard.current[Key.R].wasPressedThisFrame)
                CurrentRotateIndex = (CurrentRotateIndex + 1) % rotateMap.Count;
            if(Keyboard.current[Key.T].wasPressedThisFrame)
                CurrentRotateIndex = (CurrentRotateIndex - 1 + rotateMap.Count) % rotateMap.Count;
        }

        private void ForwardPlacePart()
        {
            DirPlacePart(Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized);
        }

        private void RightPlacePart()
        {
            DirPlacePart(UnityEngine.Camera.main.transform.right);
        }

        private void BackPlacePart()
        {
            DirPlacePart(UnityEngine.Camera.main.transform.forward * -1);
        }

        private void LeftPlacePart()
        {
            DirPlacePart(UnityEngine.Camera.main.transform.right * -1);
        }

        private void DirPlacePart(Vector3 dir)
        {
            if (CurrentOnThePart == null) return;
            var onThePartPosition = CurrentOnThePart.transform.position;
            var connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, dir,
                CurrentOnThePart.OnGetConnectPoints().Select(x => CurrentOnThePart.transform.TransformPoint(x)));
            Place(connectPoint, CurrentRotateMatrix());
        }

        private void Place(Vector3 connectPoint, Matrix4x4 multiplePartAreaMatrix)
        {
            // 次の設置するパーツを取得
            var partScriptableObject = CurrentPart();

            var tryPlaceInfo = new TryPlaceInfo(partScriptableObject, connectPoint, multiplePartAreaMatrix);
            if (PlacePartService.TryPlacePartPosition(tryPlaceInfo, out var outMatrix))
            {
                _nextPartHolders[_currentSelectHolderIndex].NextPart();
                Instantiate(partScriptableObject.PartPrefab, outMatrix.GetPosition(), outMatrix.rotation);
                OnThePartChanged();
            }
        }

        private void OnThePartUpdate()
        {
            var hit = new RaycastHit[1];
            var size = Physics.RaycastNonAlloc(transform.position, Vector3.down, hit, 1f, LayerMask.GetMask("Block"));
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
            _multiplePartArea.UpdatePart(transform.position, _currentOnThePart, CurrentPart(), CurrentRotateMatrix());
        }

        private void OnSelectChanged(ChangeSelectEvent e)
        {
            OnSelectChangedEvent?.Invoke(e);
        }

        public BlockPartScriptableObject CurrentPart()
        {
            return _nextPartHolders[_currentSelectHolderIndex].CurrentPart();
        }

        [SerializeField] private PartListScriptableObject _partListScriptableObject;


        private List<NextPartHolder> _nextPartHolders = new();
        public IEnumerable<NextPartHolder> NextPartHolders => _nextPartHolders;
        private int _currentSelectHolderIndex = 0;

        public int CurrentSelectHolderIndex
        {
            get => _currentSelectHolderIndex;
            private set
            {
                var e = new ChangeSelectEvent(value, _currentSelectHolderIndex);
                _currentSelectHolderIndex = value;
                OnThePartChanged();
                OnSelectChanged(e);
            }
        }

        [SerializeField] private PartView _currentOnThePart;
        [SerializeField] private MultiplePartArea _multiplePartArea;

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
            return Matrix4x4.Rotate(rotateMap[_currentRotateIndex]);
        }

        private static List<Quaternion> rotateMap = new List<Quaternion>()
        {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, 180, 0),
            Quaternion.Euler(0, -90, 0),
        };
    }
}