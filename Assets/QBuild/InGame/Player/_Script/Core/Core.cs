using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    private List<CoreComponent> CoreComponents = new List<CoreComponent>();

    /// <summary>
    /// コアのアップデート処理
    /// </summary>
    public void Update()
    {
        foreach (CoreComponent component in CoreComponents)
        {
            component.LogicUpdate();
        }
    }

    /// <summary>
    /// Coreに引数のComponentの追加
    /// </summary>
    /// <param name="addComponent">追加するコンポーネント</param>
    public void AddCoreComponent(CoreComponent addComponent)
    {
        if(!CoreComponents.Contains(addComponent))
            CoreComponents.Add(addComponent);
    }

    /// <summary>
    /// Coreのリストに存在するコンポーネントを返す
    /// </summary>
    /// <typeparam name="T">検索するコンポーネントの種類</typeparam>
    /// <returns>コンポーネント</returns>
    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = CoreComponents.OfType<T>().FirstOrDefault();

        if (comp == null)
            Debug.LogWarning($"{typeof(T)} が {transform.parent.name}　に見つかりません。");

        return comp;
    }

    /// <summary>
    /// Coreのリストに存在するコンポーネントを返す
    /// </summary>
    /// <typeparam name="T">検索するコンポーネントの種類</typeparam>
    /// <param name="value">コンポーネントの参照渡し</param>
    /// <returns>コンポーネント</returns>
    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
