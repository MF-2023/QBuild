using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.StageEditor
{
    public class CheckNormalStageData : MonoBehaviour
    {
        private const string StartTag = "Start";
        private const string GoalTag = "Goal";

        public struct WarningLog
        {
            public string text;
            public List<GameObject> targetObject;
        }

        private static WarningLog AddWarningLog(string log, List<GameObject> obj)
        {
            List<GameObject> o = obj ?? new List<GameObject>();

            var errorLog = new WarningLog
            {
                text = log,
                targetObject = o
            };
            return errorLog;
        }

        public static List<WarningLog> CheckStageData(StageData stageData)
        {
            List<WarningLog> errorLogList = new List<WarningLog>();

            if (stageData == null)
            {
                return errorLogList;
            }

            //stageが設定されているか
            if (stageData.GetStagePrefab() == null)
            {
                errorLogList.Add(AddWarningLog("Blockが設置されていません", null));
            }

            //ファイル名が設定されているか
            if (stageData.GetFileName() == "")
            {
                errorLogList.Add(AddWarningLog("fileNameが設定されていません", null));
            }

            //ステージ名が設定されているか
            if (stageData.GetStageName() == "")
            {
                errorLogList.Add(AddWarningLog("stageNameが設定されていません", null));
            }

            //クリスタルの数が設定されているか
            if (stageData.GetStageImage() == null)
            {
                errorLogList.Add(AddWarningLog("stageImageが設定されていません", null));
            }

            //ステージの大きさが設定されているか
            if (stageData.GetStageArea().x == 0 || stageData.GetStageArea().y == 0 || stageData.GetStageArea().z == 0)
            {
                errorLogList.Add(AddWarningLog("stageAreaが設定されていません", null));
            }

            var blocks = StageEditorWindow.GetAllBlocksInScene();

            /*
            //スタート地点があるか
            if (blocks.All(block => !block.CompareTag(StartTag)))
            {
                errorLogList.Add(AddErrorLog("スタート地点がありません", null));
            }

            //ゴール地点があるか
            if (blocks.All(block => !block.CompareTag(GoalTag)))
            {
                errorLogList.Add(AddErrorLog("ゴール地点がありません", null));
            }
            */

            //全てのブロックが重なっていないか
            if (CheckBlockOverlap(blocks, out var overlappingBlocks))
            {
                errorLogList.Add(AddWarningLog("ブロックが重なっています", overlappingBlocks));
            }

            return errorLogList;
        }


        private static bool CheckBlockOverlap(List<GameObject> blocks, out List<GameObject> overlappingBlocks)
        {
            var overlapBlockList = new List<GameObject>();

            foreach (var block in blocks)
            {
                if (!block.TryGetComponent(out Collider collider)) continue;

                foreach (var otherBlock in blocks.Where(otherBlock => block != otherBlock))
                {
                    if (!otherBlock.TryGetComponent(out Collider otherCollider)) continue;

                    var blockBounds = collider.bounds;
                    var otherBlockBounds = otherCollider.bounds;
                    //少し狭めて判定
                    blockBounds.Expand(-0.1f);
                    otherBlockBounds.Expand(-0.1f);
                    if (blockBounds.Intersects(otherBlockBounds))
                    {
                        overlapBlockList.Add(block);
                        break;
                    }
                }
            }

            overlappingBlocks = overlapBlockList;
            return overlappingBlocks.Count > 0;
        }
    }
}