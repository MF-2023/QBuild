using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoVariableTool;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultEffect : MonoBehaviour
    {
        [SerializeField] private ResultPopup _resultPopup;

        public void StartResult()
        {
            
            ResultEffectStart(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask ResultEffectStart(CancellationToken token)
        {
            //プレイヤーの取得どうする？
            //await キャラクターアニメション
            await _resultPopup.Show(token);
            await UniTask.WaitUntil(() => _resultPopup.IsClickAny, cancellationToken: token);
            
            //ポップアップ閉じる?
            await _resultPopup.Close(token);
        }
    }
}