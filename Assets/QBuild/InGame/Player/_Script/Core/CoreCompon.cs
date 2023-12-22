using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.Core
{
    public class CoreComponent : MonoBehaviour
    {
        private Core core;

        /// <summary>
        /// �I�u�W�F�N�g��������Core�ɃR���|�[�l���g�ǉ�
        /// </summary>
        protected virtual void Awake()
        {
            core = transform.parent.GetComponent<Core>();

            if (core == null) UnityEngine.Debug.LogError(transform.root.gameObject.name + "��Core�����݂��܂���B");
            else core.AddCoreComponent(this);
        }

        /// <summary>
        /// �R���|�[�l���g�̃A�b�v�f�[�g����
        /// </summary>
        public virtual void CompLogicUpdate() { }
        
        /// <summary>
        /// �R���|�[�l���g��FixedUpdate����
        /// </summary>
        public virtual void CompFixedUpdate() {}
    }
}
