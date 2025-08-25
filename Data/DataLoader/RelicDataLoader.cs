using System.Collections.Generic;
using UnityEngine;

public static class RelicDataLoader
{
    [System.Serializable]
    private class RelicDataList
    {
        public List<RelicData> relics;
    }

    public static List<RelicData> LoadRelicData(TextAsset relicJsonFile)
    {
        if(relicJsonFile == null)
        {
            return new List<RelicData> ();
        }

        var dataList = JsonUtility.FromJson<RelicDataList>(relicJsonFile.text);
        return dataList?.relics ?? new List<RelicData>();
    }
}