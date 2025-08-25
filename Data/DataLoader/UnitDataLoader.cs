using System.Collections.Generic;
using UnityEngine;

public static class UnitDataLoader
{
    [System.Serializable]
    private class UnitDataList
    {
        public List<UnitData> units;
    }

    public static List<UnitData> LoadUnitData(TextAsset unitJsonFile)
    {
        if(unitJsonFile == null)
        {
            return new List<UnitData>();
        }

        var dataList = JsonUtility.FromJson<UnitDataList>(unitJsonFile.text);
        return dataList?.units ?? new List<UnitData>();
    }
}
