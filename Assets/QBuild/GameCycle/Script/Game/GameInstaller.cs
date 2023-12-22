using QBuild.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.GameCycle
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private SelectStageSO _selectStageSO;

        private void Awake()
        {
            /*
            // �X�e�[�W�̐���
            if (_selectStageSO.SelectStagePrefab == null)
            {
                //Instantiate(_initStagePrefab);
            }
            else
            {
                Instantiate(_selectStageSO.SelectStagePrefab);
                //_selectStageSO.SelectStagePrefab = null;
            }
            */
        }
    }
}
