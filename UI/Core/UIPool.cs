using System.Collections.Generic;
using UnityEngine;

public class UIPool
{
    private readonly Dictionary<UIType, Queue<BaseUI>> pool = new();

    public BaseUI GetUI(UIType type, Transform parent)
    {
        if (pool.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            var ui = queue.Dequeue();
            ui.transform.SetParent(parent, false);
            ui.gameObject.SetActive(true);
            return ui;
        }

        string path = UIPath.GetPath(type);
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        return go.GetComponent<BaseUI>();
    }

    public void ReturnUI(UIType type, BaseUI ui)
    {
        ui.gameObject.SetActive(false);
        if (!pool.ContainsKey(type))
            pool[type] = new Queue<BaseUI>();
        pool[type].Enqueue(ui);
    }
}
