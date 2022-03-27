using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    // 核心元件
    public readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();


    public void LogicUpdate()// 邏輯更新
    {
        foreach (var component in CoreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(CoreComponent component)// 增加 CoreComponent 元件到 List 以遍歷更新
    {
        if (!CoreComponents.Contains(component))
        {
            CoreComponents.Add(component);
        }
    }

    public T GetCoreComponent<T>() where T:CoreComponent
    {
        // 在 list 尋找需要的 component
        var comp = CoreComponents
            .OfType<T>()
            .FirstOrDefault();

        // 確認是否有找到
        if(comp == null)
        {
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        }

        // 回傳 component
        return comp;
    }
    
    public T GetCoreComponent<T>(ref T value) where T:CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
