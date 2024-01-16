using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultEffect : MonoBehaviour
    {
        [SerializeField] private ResultPopup _resultPopup;
        
        public async UniTask ResultEffectStart()
        {
            //await キャラクターアニメション
            await _resultPopup.Show();
            await UniTask.WaitUntil(() => _resultPopup.IsClickAny);
            
            //PopUp閉じる？
            //_resultPopup.Close();
        }
    }
}