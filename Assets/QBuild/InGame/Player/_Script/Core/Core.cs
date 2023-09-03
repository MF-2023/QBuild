using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    private List<CoreComponent> CoreComponents = new List<CoreComponent>();

    /// <summary>
    /// �R�A�̃A�b�v�f�[�g����
    /// </summary>
    public void Update()
    {
        foreach (CoreComponent component in CoreComponents)
        {
            component.LogicUpdate();
        }
    }

    /// <summary>
    /// Core�Ɉ�����Component�̒ǉ�
    /// </summary>
    /// <param name="addComponent">�ǉ�����R���|�[�l���g</param>
    public void AddCoreComponent(CoreComponent addComponent)
    {
        if(!CoreComponents.Contains(addComponent))
            CoreComponents.Add(addComponent);
    }

    /// <summary>
    /// Core�̃��X�g�ɑ��݂���R���|�[�l���g��Ԃ�
    /// </summary>
    /// <typeparam name="T">��������R���|�[�l���g�̎��</typeparam>
    /// <returns>�R���|�[�l���g</returns>
    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = CoreComponents.OfType<T>().FirstOrDefault();

        if (comp == null)
            Debug.LogWarning($"{typeof(T)} �� {transform.parent.name}�@�Ɍ�����܂���B");

        return comp;
    }

    /// <summary>
    /// Core�̃��X�g�ɑ��݂���R���|�[�l���g��Ԃ�
    /// </summary>
    /// <typeparam name="T">��������R���|�[�l���g�̎��</typeparam>
    /// <param name="value">�R���|�[�l���g�̎Q�Ɠn��</param>
    /// <returns>�R���|�[�l���g</returns>
    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
