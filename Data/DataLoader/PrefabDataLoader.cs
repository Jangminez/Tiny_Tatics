using System.Collections.Generic;
using UnityEngine;

public class PrefabDataLoader
{
    private Dictionary<string, GameObject> prefabDict;

    public PrefabDataLoader(List<GameObject> prefabs)
    {
        prefabDict = new Dictionary<string, GameObject>();

        foreach (var prefab in prefabs)
        {
            if (prefab.TryGetComponent(out IDHolder idHolder))
            {
                if (!prefabDict.ContainsKey(idHolder.id))
                {
                    prefabDict.Add(idHolder.id, prefab);
                }
                else
                {
                    Debug.LogWarning($"중복된 ID가 발견되었습니다: {idHolder.id}");
                }
            }
        }
    }

    public GameObject GetByID(string id)
    {
        prefabDict.TryGetValue(id, out GameObject obj);
        return obj;
    }
}
