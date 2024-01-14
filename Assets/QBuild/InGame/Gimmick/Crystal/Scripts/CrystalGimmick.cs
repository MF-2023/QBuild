using SoVariableTool;
using UnityEngine;

namespace QBuild.Gimmick.Crystal
{
    public class CrystalGimmick : MonoBehaviour
    {
        [SerializeField] private Int32ScriptableEventObject _crystalCount;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            _crystalCount.Raise(1);
            gameObject.SetActive(false);
        }
    }
}