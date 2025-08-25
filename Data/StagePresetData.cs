using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitPresetData
{
    public ObjectType type;      // "monster", "turret", "obstacle"
    public string id;           // 고유 프리팹 ID
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    public UnitPresetData(ObjectType type, string id, SerializableVector3 position, SerializableQuaternion rotation)
    {
        this.type = type;
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }
}

[System.Serializable]
public class StagePresetData
{
    public string stageID;
    public string baseMapID;
    public List<UnitPresetData> units = new List<UnitPresetData>();

    public StagePresetData(string stageID, string baseMapID, List<UnitPresetData> units)
    {
        this.stageID = stageID;
        this.baseMapID = baseMapID;
        this.units = units;
    }
}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

[System.Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public SerializableQuaternion(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }

    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
}

public class StagePresetLoader
{
    public List<StagePresetData> StageLists { get; private set; }
    public List<StagePresetData> BossStageLists { get; private set; }
    public Dictionary<string, StagePresetData> StageDict { get; private set; }

    public StagePresetLoader(string path = "Data/StageData/")
    {
        StageLists = new List<StagePresetData>();
        BossStageLists = new List<StagePresetData>();
        StageDict = new Dictionary<string, StagePresetData>();

        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>(path);

        foreach (var file in jsonFiles)
        {
            StagePresetData data = JsonUtility.FromJson<StagePresetData>(file.text);

            if (data != null)
            {
                if (data.stageID.Contains("boss"))
                    BossStageLists.Add(data);
                else
                    StageLists.Add(data);
            }
        }
    }

    public StagePresetData GetByID(string stageID)
    {
        StageDict.TryGetValue(stageID, out StagePresetData data);
        return data;
    }

    public StagePresetData GetRandomStage()
    {
        StagePresetData stagePreset = StageLists[Random.Range(0, StageLists.Count)];
        StageLists.Remove(stagePreset);
        
        return stagePreset;
    }

    public StagePresetData GetBossStage()
    {
        return BossStageLists[Random.Range(0, BossStageLists.Count)];
    }
}
