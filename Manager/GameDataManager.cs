using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public TextAsset relicJsonFile;
    public TextAsset unitJsonFile;
    public TextAsset cardJsonFile;
    public TextAsset buildingJsonFile;

    public Dictionary<string, RelicData> RelicsById { get; private set; } = new();
    public Dictionary<string, UnitData> UnitsById { get; private set; } = new();
    public Dictionary<string, CardData> CardsById { get; private set; } = new();
    public Dictionary<string, BuildingData> BuildingsById { get; private set; } = new();


    public IReadOnlyList<RelicData> AllRelics => new List<RelicData>(RelicsById.Values);
    public IReadOnlyList<UnitData> AllUnits => new List<UnitData>(UnitsById.Values);
    public IReadOnlyList<CardData> AllCards => new List<CardData>(CardsById.Values);

    public void Init()
    {   
        LoadAllData();
    }

    private void LoadAllData()
    {
        var relicList = RelicDataLoader.LoadRelicData(relicJsonFile);
        foreach (var relic in relicList)
        {
            RelicsById[relic.relicId] = relic;
        }

        var unitList = UnitDataLoader.LoadUnitData(unitJsonFile);
        foreach (var unit in unitList)
        {
            UnitsById[unit.unitId] = unit;
        }

        var cardList = CardDataLoader.LoadCardData(cardJsonFile);
        foreach (var card in cardList)
        {
            CardsById[card.cardId] = card;
        }
    }

    public UnitData GetUnitById(string id) => UnitsById.TryGetValue(id, out var data) ? data : null;
    public CardData GetCardById(string id) => CardsById.TryGetValue(id, out var data) ? data : null;
}
