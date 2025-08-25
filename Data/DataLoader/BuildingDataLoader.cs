using System.Collections.Generic;
using UnityEngine;

public static class BuildingDataLoader
{
    [System.Serializable]
    private class BuildingDataList
    {
        public List<BuildingData> buildings;
    }

    public static List<BuildingData> LoadBuildingData(TextAsset buildingJsonFile)
    {
        if(buildingJsonFile == null)
        {
            return new List<BuildingData> ();
        }

        var dataList = JsonUtility.FromJson<BuildingDataList>(buildingJsonFile.text);
        return dataList?.buildings ?? new List<BuildingData>();
    }
}
