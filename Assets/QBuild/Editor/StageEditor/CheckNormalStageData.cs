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

            //stage���ݒ肳��Ă��邩
            if (stageData.GetStagePrefab() == null)
            {
                errorLogList.Add(AddWarningLog("Block���ݒu����Ă��܂���", null));
            }

            //�t�@�C�������ݒ肳��Ă��邩
            if (stageData.GetFileName() == "")
            {
                errorLogList.Add(AddWarningLog("fileName���ݒ肳��Ă��܂���", null));
            }

            //�X�e�[�W�����ݒ肳��Ă��邩
            if (stageData.GetStageName() == "")
            {
                errorLogList.Add(AddWarningLog("stageName���ݒ肳��Ă��܂���", null));
            }

            //�N���X�^���̐����ݒ肳��Ă��邩
            if (stageData.GetStageImage() == null)
            {
                errorLogList.Add(AddWarningLog("stageImage���ݒ肳��Ă��܂���", null));
            }

            //�X�e�[�W�̑傫�����ݒ肳��Ă��邩
            if (stageData.GetStageArea().x == 0 || stageData.GetStageArea().y == 0 || stageData.GetStageArea().z == 0)
            {
                errorLogList.Add(AddWarningLog("stageArea���ݒ肳��Ă��܂���", null));
            }

            var blocks = StageEditorWindow.GetAllBlocksInScene();

            /*
            //�X�^�[�g�n�_�����邩
            if (blocks.All(block => !block.CompareTag(StartTag)))
            {
                errorLogList.Add(AddErrorLog("�X�^�[�g�n�_������܂���", null));
            }

            //�S�[���n�_�����邩
            if (blocks.All(block => !block.CompareTag(GoalTag)))
            {
                errorLogList.Add(AddErrorLog("�S�[���n�_������܂���", null));
            }
            */

            //�S�Ẵu���b�N���d�Ȃ��Ă��Ȃ���
            if (CheckBlockOverlap(blocks, out var overlappingBlocks))
            {
                errorLogList.Add(AddWarningLog("�u���b�N���d�Ȃ��Ă��܂�", overlappingBlocks));
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
                    //�������߂Ĕ���
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