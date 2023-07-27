using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    private Core core;

    /// <summary>
    /// �I�u�W�F�N�g��������Core�ɃR���|�[�l���g�ǉ�
    /// </summary>
    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();

        if (core == null) Debug.LogError(transform.root.gameObject.name + "��Core�����݂��܂���B");
        else core.AddCoreComponent(this);
    }

    /// <summary>
    /// �R���|�[�l���g�̃A�b�v�f�[�g����
    /// </summary>
    public virtual void LogicUpdate() { }
}
