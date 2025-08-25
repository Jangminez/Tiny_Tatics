using System.Collections.Generic;
using UnityEngine;

public static class CardDataLoader
{
    [System.Serializable]
    private class CardDataList
    {
        public List<CardData> cards;
    }


    public static List<CardData> LoadCardData(TextAsset cardJsonFile)
    {
        if(cardJsonFile == null)
        {
            return new List<CardData>();
        }

        var dataList = JsonUtility.FromJson<CardDataList>(cardJsonFile.text);
        return dataList?.cards ?? new List<CardData>();
    }
}
