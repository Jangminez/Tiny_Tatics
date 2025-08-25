using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : MonoBehaviour
{
    public StagePresetLoader StageLoader { get; private set; }
    public PrefabDataLoader PrefabLoader { get; private set; }
    public MapDataLoader MapLoader { get; private set; }

    public List<GameObject> allPrefabs;

    void Awake()
    {
        StageLoader = new StagePresetLoader();
        PrefabLoader = new PrefabDataLoader(allPrefabs);
        MapLoader = new MapDataLoader();
    }
}
