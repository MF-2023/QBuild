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
            Debug.Log(Direction);
            
            _cancellationTokenSource = new CancellationTokenSource();

            _channel = Channel.CreateSingleConsumerUnbounded<ShiftDirectionTimes>();
            var reader = _channel.Reader;
            WaitForChannelAsync(reader, _cancellationTokenSource.Token).Forget();
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
    }
}