using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Unit,
    Spell,
    Buff,
    Structure,
}

[System.Serializable]
public class CardData
{
    public string cardId;
    public string unitId;
    public string cardName;
    public string cardType;
    public string description;
    public string iconPath;
    public int cost;
    public int price;
    public float spawnDelay;
    public bool reusable;
    public List<CardSpawnUnitInfo> spawnUnits;
    public string useSfxPath;
    public string playEffectPath;

    // Cached parsed CardType enum
    [System.NonSerialized]
    private CardType? _cardTypeEnum;
    public CardType CardTypeEnum
    {
        get
        {
            if (_cardTypeEnum == null)
            {
                if (!System.Enum.TryParse(cardType, true, out CardType parsed))
                {
                    parsed = CardType.Unit;
                }
                _cardTypeEnum = parsed;
            }
            return _cardTypeEnum.Value;
        }
    }
}

[System.Serializable]
public class CardSpawnUnitInfo
{
    public string unitPrefabPath;
    public int count;
    public List<Vector3Data> spawnOffsets;
}

[System.Serializable]
public class Vector3Data
{
    public float x,
        y,
        z;

    public Vector3 ToVector3() => new Vector3(x, y, z);
}
