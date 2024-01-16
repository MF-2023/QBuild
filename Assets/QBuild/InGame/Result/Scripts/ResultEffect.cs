using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultEffect : MonoBehaviour
    {
        [SerializeField] private ResultPopup _resultPopup;

        private void Start()
        {
            ResultEffectTest();
        }

        public async UniTask ResultEffectStart()
        {
            //プレイヤーの取得どうする？
            //await キャラクターアニメション
            await _resultPopup.Show();
            await UniTask.WaitUntil(() => _resultPopup.IsClickAny);
            
            _resultPopup.Close();
        }

        private async UniTask ResultEffectTest()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            await ResultEffectStart();
            Debug.Log("ResultEffect終了");
        }
    }
}