using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    private Core core;

    /// <summary>
    /// オブジェクト生成時にCoreにコンポーネント追加
    /// </summary>
    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();

        if (core == null) Debug.LogError(transform.root.gameObject.name + "にCoreが存在しません。");
        else core.AddCoreComponent(this);
    }

    /// <summary>
    /// コンポーネントのアップデート処理
    /// </summary>
    public virtual void LogicUpdate() { }
}
