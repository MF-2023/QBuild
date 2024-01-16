using System;
using Cysharp.Threading.Tasks;
using SoVariableTool;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultEffect : MonoBehaviour
    {
        [SerializeField] private ResultPopup _resultPopup;
        private UniTask _token;

        public void StartResult()
        {
            _token = ResultEffectStart();
        }
        
        public async UniTask ResultEffectStart()
        {
            //プレイヤーの取得どうする？
            //await キャラクターアニメション
            await _resultPopup.Show();
            await UniTask.WaitUntil(() => _resultPopup.IsClickAny);
            
            //ポップアップ閉じる?
            await _resultPopup.Close();
        }
    }
}