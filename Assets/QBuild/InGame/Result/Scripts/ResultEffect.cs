using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using QBuild.Player;
using SoVariableTool;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultEffect : MonoBehaviour
    {
        [SerializeField] private ResultPopup _resultPopup;
        [SerializeField] private PlayerCurrentData _playerProgressData;

        public void StartGoalResult()
        {
            Debug.Log("Start Goal Result");
            ResultEffectStartGoal(this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        public void StartFailedResult()
        {
            Debug.Log("Start Failed Result");
            ResultEffectStartFailed(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask ResultEffectStartGoal(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _playerProgressData.EndGoalAnimation, cancellationToken: token);
            await _resultPopup.GoalShow(token);
            await UniTask.WaitUntil(() => _resultPopup.IsClickAny, cancellationToken: token);
            //await _resultPopup.Close(token);
        }

        private async UniTask ResultEffectStartFailed(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _playerProgressData.EndFailedAnimation, cancellationToken: token);
            await _resultPopup.FailedShow(token);
            await UniTask.WaitUntil(() => _resultPopup.IsClickAny, cancellationToken: token);
        }
    }
}