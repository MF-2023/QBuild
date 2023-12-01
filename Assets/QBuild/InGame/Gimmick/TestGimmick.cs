using System;
using QBuild.Gimmick.Effector;
using QBuild.Gimmick.TriggerPattern;
using QBuild.Player.Controller;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class TestGimmick : BaseGimmick
    {
        private PlayerController _playerController;

        private void Start()
        {
            this.OnContacted(x =>
            {
                Debug.Log("OnContacted");
                _playerController = x.Target.GetComponent<PlayerController>();
            });
        }

        public override void Active()
        {
            Debug.Log("Active");
            Debug.Log(_playerController.name);
        }

        public override void Disable()
        {
            Debug.Log("Disable");
        }

        public void Test()
        {
            Debug.Log("Test");
        }
    }
}