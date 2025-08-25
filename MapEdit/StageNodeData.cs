using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageNodeData
{
    public string nodeID;
    public NodeType type;
    public Vector2 uiPosition;
    public List<string> connectedNodeIds;
}

[System.Serializable]
public class StageMapData
{
    public string mapID;
    public List<StageNodeData> nodes;
}

public class MapDataLoader
{
    public List<StageMapData> MapLists { get; private set; }
    public Dictionary<string, StageMapData> MapDict { get; private set; }

    public MapDataLoader(string path = "Data/MapData/")
    {
        MapLists = new List<StageMapData>();
        MapDict = new Dictionary<string, StageMapData>();

        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>(path);

        foreach (var file in jsonFiles)
        {
            StageMapData data = JsonUtility.FromJson<StageMapData>(file.text);

            if (data != null)
            {
                MapLists.Add(data);
                MapDict.Add(data.mapID, data);
            }
        }
    }

    public StageMapData GetByID(string mapID)
    {
        MapDict.TryGetValue(mapID, out StageMapData data);
        return data;
    }

    public StageMapData GetRandomMap()
    {
        return MapLists[Random.Range(0, MapLists.Count)];
    }
}
