using SoVariableTool;
using UnityEngine;

namespace QBuild.Gimmick.Crystal
{
    public class CrystalRegisterAdapter : MonoBehaviour
    {
        [SerializeField] private CrystalSlots _crystalSlots;
        
        private void Start()
        {
            var count = FindObjectsByType(typeof (CrystalGimmick), FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
            _crystalSlots.SetMaxCrystalCount(count);
        }
        
        public void OnGetCrystal()
        {
            _crystalSlots.OnGetCrystal();
        }
    }
}