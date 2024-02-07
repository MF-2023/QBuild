using QBuild.Grid;
using QBuild.Player.Controller;
using UnityEngine;

namespace QBuild.Stage.Grid
{
    public class GridViewControl : MonoBehaviour
    {
        [SerializeField] private SelectStageSO _selectStageSO;
        private PlayerController _playerController;
        private DrawGrid _drawGrid;
        
        public void Bind()
        {
            _drawGrid = FindObjectOfType<DrawGrid>();
            _drawGrid.SetStageData(_selectStageSO.SelectStageData);
            _playerController = FindObjectOfType<PlayerController>();
            _playerController.OnChangeGridPosition += ChangeGridPosition;
        }

        private void ChangeGridPosition(Vector3 position)
        {
            _drawGrid.SetPlayerPositionY(position.y);
        }
    }
}