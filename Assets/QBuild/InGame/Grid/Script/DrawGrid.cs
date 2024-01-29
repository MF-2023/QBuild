using System;
using System.Collections;
using System.Collections.Generic;
using QBuild.StageEditor;
using UnityEngine;

namespace QBuild.Grid
{
    /// <summary>
    /// グリッドを描画するクラス
    /// StageDataとPlayerPositionYを受け取り、グリッドを描画する
    /// セッター；
    ///     StageData
    ///     PlayerPositionY
    /// </summary>
    public class DrawGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _gridPrefab;

        private readonly List<GameObject> _gridList = new();

        private StageData _stageData;

        private int _playerPositionY;
        private const int GridInterval = 1;
        private const float LineWidth = 0.05f;

        /// <summary>
        /// StageDataをセットする
        /// </summary>
        /// <param name="stageData"></param>
        public void SetStageData(StageData stageData)
        {
            _stageData = stageData;
            Draw();
        }

        /// <summary>
        /// PlayerPositionYをセットする
        /// </summary>
        /// <param name="posY"></param>
        public void SetPlayerPositionY(float posY)
        {
            if (_stageData == null)
            {
                Debug.LogError("StageData is null");
                return;
            }

            int correctionPosY = Mathf.RoundToInt(posY);
            correctionPosY = Mathf.Clamp(correctionPosY, 0, _stageData.GetStageArea().z - 1);
            if (_playerPositionY == correctionPosY) return;
            _playerPositionY = correctionPosY;
            Draw(correctionPosY);
        }

        private void Draw(int posY = 0)
        {
            if (_stageData == null)
            {
                Debug.LogError("StageData is null");
                return;
            }

            if (_gridList.Count == 0)
            {
                InstantiateGrid(posY);
            }
            else
            {
                MoveGrid(posY);
            }
        }

        private void InstantiateGrid(int posY)
        {
            var areaX = _stageData.GetStageArea().x;
            var areaZ = _stageData.GetStageArea().z;
            var areaXHalf = areaX / 2;
            var areaZHalf = areaZ / 2;
            float offset = GridInterval / 2.0f;

            //グリッドを描画
            for (int i = -areaXHalf; i < areaXHalf; i++)
            {
                for (int j = -areaZHalf; j < areaZHalf; j++)
                {
                    var grid = Instantiate(_gridPrefab, transform);
                    grid.transform.localPosition = new Vector3(i + offset, posY, j + offset);
                    _gridList.Add(grid);
                }
            }

            //外側の枠線を描画
            var obj = new GameObject("Line");
            obj.transform.SetParent(transform);
            obj.AddComponent<BillboardRenderer>();
            var line = obj.AddComponent<LineRenderer>();
            line.positionCount = 4;
            line.loop = true;
            line.startWidth = LineWidth;
            line.endWidth = LineWidth;
            line.useWorldSpace = false;
            line.numCornerVertices = 90;
            line.material = _gridList[0].GetComponent<MeshRenderer>().material;
            line.SetPosition(0, new Vector3(-areaXHalf - offset, posY, -areaZHalf - offset));
            line.SetPosition(1, new Vector3(-areaXHalf - offset, posY, areaZHalf + offset));
            line.SetPosition(2, new Vector3(areaXHalf + offset, posY, areaZHalf + offset));
            line.SetPosition(3, new Vector3(areaXHalf + offset, posY, -areaZHalf - offset));
            _gridList.Add(obj);
        }

        private void MoveGrid(int posY)
        {
            foreach (var grid in _gridList)
            {
                var localPosition = grid.transform.localPosition;
                localPosition = new Vector3(localPosition.x, posY, localPosition.z);
                grid.transform.localPosition = localPosition;
            }
        }
    }
}