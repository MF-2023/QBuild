using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using QBuild.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QBuild.Part
{
    [RequireComponent(typeof(Connector))]
    public class PartView : MonoBehaviour
    {
        public DirectionFRBL Direction { get; set; } = DirectionFRBL.Forward;

        private Connector _connector;
        private Channel<ShiftDirectionTimes> _channel;
        private CancellationTokenSource _cancellationTokenSource;

        public void Awake()
        {
            var r = transform.localRotation;
            var e = r.eulerAngles;
            Direction = DirectionFRBLExtension.VectorToDirectionFRBL(e);

            _cancellationTokenSource = new CancellationTokenSource();

            _channel = Channel.CreateSingleConsumerUnbounded<ShiftDirectionTimes>();
            var reader = _channel.Reader;
            WaitForChannelAsync(reader, _cancellationTokenSource.Token).Forget();
            ContactUpdate();
        }

        private void OnDestroy()
        {
            _channel.Writer.TryComplete();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public IEnumerable<Vector3> OnGetConnectPoints()
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            return _connector.ConnectPoints();
        }

        public IEnumerable<PartConnectPoint.Magnet> OnGetMagnets()
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            return _connector.ConnectMagnet();
        }

        public bool TryGetConnectPoint(DirectionFRBL direction, out Vector3 position)
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            if (gameObject.activeInHierarchy) ContactUpdate();
            return _connector.TryGetConnectPoint(direction, out position);
        }

        public bool HasDirection(DirectionFRBL direction)
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            return _connector.HasDirection(direction);
        }

        public void SetCanConnect(DirectionFRBL dir, bool canConnect)
        {
            if (_connector == null)
            {
                _connector = GetComponent<Connector>();
            }

            var shiftedDir = DirectionUtilities.CalcDirectionFRBL(Direction, dir);
            _connector.SetCanConnect(shiftedDir, canConnect);
        }

        private async UniTaskVoid WaitForChannelAsync(ChannelReader<ShiftDirectionTimes> reader,
            CancellationToken cancellationToken)
        {
            try
            {
                await reader.ReadAllAsync()
                    .ForEachAwaitAsync(async x => { await TurnAsync(x); }, cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Turn(ShiftDirectionTimes times)
        {
            _channel.Writer.TryWrite(times);
        }

        public async Task TurnAsync(ShiftDirectionTimes times)
        {
            var turnDirection = Direction.Shift(times);
            Direction = turnDirection;
            var rot = transform.localRotation.eulerAngles;
            rot.y += times.Value * 90;
            await transform.DOLocalRotate(rot, 0.5f).AsyncWaitForCompletion();
        }

        private void ContactUpdate()
        {
            foreach (var magnet in OnGetMagnets())
            {
                var position = transform.TransformPoint(magnet.Position);
                position.y -= 0.1f;
                var dirRay = magnet.Direction.ToVector3();
                dirRay = transform.TransformDirection(dirRay).normalized;
                var ray = new Ray(position - dirRay * 0.1f, dirRay);

                // Blockに接触している
                var contact = Physics.Raycast(ray, out var hit, 1f, LayerMask.GetMask("Block"));
                var shift = new ShiftDirectionTimes((((int)Direction - 1)));
                if (Input.GetKey(KeyCode.B))
                {
                    Debug.DrawRay(ray.origin + Vector3.up, ray.direction + Vector3.up, Color.HSVToRGB((int)magnet.Direction / 4.0f,1,1), 50f);
                    var obj1 = new GameObject($"Test1 S:{magnet.Direction}");
                    obj1.transform.position = ray.origin + Vector3.up;
                    var obj2 = new GameObject($"Test1 E:{magnet.Direction}");
                    obj2.transform.position = ray.GetPoint(1) + Vector3.up;
                }
                if (contact)
                    SetCanConnect(magnet.Direction.Shift(shift), !contact);
            }
        }
    }
}