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
        [SerializeField] private PlayerProgressData _playerProgressData;

        public void StartResult()
        {
            
            ResultEffectStart(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask ResultEffectStart(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _playerProgressData.EndGoalAnimation, cancellationToken: token);
            
            await _resultPopup.Show(token);
            await UniTask.WaitUntil(() => _resultPopup.IsClickAny, cancellationToken: token);
            
            //ポップアップ閉じる?
            await _resultPopup.Close(token);
        }
    }
}