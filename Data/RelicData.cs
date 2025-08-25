using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#region relic enums
public enum RelicEffectType
{
    AttackDamage,
    MoveSpeed,
    AttackSpeed,
    ManaRegen,
}

public enum RelicTargetType
{
    Unit,
    Player,
}

public enum RelicRarity
{
    UnCommon,
    Common,
    Rare,
    Epic,
    Legendary,
}

public enum RelicEffectTarget
{
    All,
    Melee,
    Ranged,
}

public enum RelicType
{
    Consumable,
    Passive,
    Conditional,
}
#endregion

[System.Serializable]
public class RelicData
{
    public string relicId;
    public string relicName;
    public string description;
    public string iconPath;

    public string effectTarget;
    public string effectType;
    public string targetType;
    public float effectValue;

    public int price;
    public string rarity;

    public bool isUnique;
    public bool isPassive;

    public int maxStack = 1;

    public string relicType;

    [TextArea(2, 4)]
    public string flavorText;

    #region Enum Caching
    [System.NonSerialized]
    private RelicEffectType? _effectTypeEnum;
    public RelicEffectType EffectTypeEnum
    {
        get
        {
            if(_effectTypeEnum == null)
            {
                if(!System.Enum.TryParse(effectType, true, out RelicEffectType parsed))
                {
                    parsed = RelicEffectType.AttackDamage;
                }
                _effectTypeEnum = parsed;
            }
            return _effectTypeEnum.Value;
        }
    }

    [System.NonSerialized]
    private RelicTargetType? _targetTypeEnum;
    public RelicTargetType TargetTypeEnum
    {
        get
        {
            if(_targetTypeEnum == null)
            {
                if(!System.Enum.TryParse(targetType, true, out RelicTargetType parsed))
                {
                    parsed = RelicTargetType.Unit;
                }
                _targetTypeEnum = parsed;
            }
            return _targetTypeEnum.Value;
        }
    }

    [System.NonSerialized]
    private RelicEffectTarget? _effectTargetEnum;
    public RelicEffectTarget EffectTargetEnum
    {
        get
        {
            if(_effectTargetEnum == null)
            {
                if(!System.Enum.TryParse(effectTarget, true, out RelicEffectTarget parsed))
                {
                    parsed = RelicEffectTarget.All;
                }
                _effectTargetEnum = parsed;
            }
            return _effectTargetEnum.Value;
        }
    }

    [System.NonSerialized]
    private RelicRarity? _rarityEnum;
    public RelicRarity RarityEnum
    {
        get
        {
            if (_rarityEnum == null)
            {
                if (!System.Enum.TryParse(rarity, true, out RelicRarity parsed))
                {
                    parsed = RelicRarity.Common;
                }
                _rarityEnum = parsed;
            }
            return _rarityEnum.Value;
        }
    }

    [System.NonSerialized]
    private RelicType? _relicTypeEnum;
    public RelicType RelicTypeEnum
    {
        get
        {
            if (_relicTypeEnum == null)
            {
                if (!System.Enum.TryParse(relicType, true, out RelicType parsed))
                {
                    parsed = RelicType.Passive;
                }
                _relicTypeEnum = parsed;
            }
            return _relicTypeEnum.Value;
        }
    }
    #endregion
}