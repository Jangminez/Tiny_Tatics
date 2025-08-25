using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region enums
public enum UnitType
{
    Ground,
    Air,
    Building,
}

public enum AttackMode
{
    Melee,
    Ranged,
    Explosive,
}

public enum AttackTargetType
{
    GroundOnly,
    AirOnly,
    Both,
    BuildingsOnly,
}

public enum UnitRarity
{
    Common,
    Rare,
    Epic,
    Legendary,
}
#endregion

[System.Serializable]
public class UnitData
{
    public string unitId;
    public string unitName;
    public string unitType;
    public string attackMode;
    public string attackTarget;
    public string rarity;

    public float hp;
    public float moveSpeed;
    public float attackDamage;
    public float attackRange;
    public float attackInterval;

    public string iconPath;
    public string prefabPath;
    public string spawnSfxPath;
    public string deathSfxPath;
    public string attackSfxPath;
    public string takeSfxPath;

    public bool isEnemy;

    #region Unit Type caching
    [NonSerialized]
    private UnitType? _unitTypeEnum;
    public UnitType UnitTypeEnum
    {
        get
        {
            if (_unitTypeEnum == null)
            {
                if (!Enum.TryParse(unitType, true, out UnitType parsed))
                {
                    parsed = UnitType.Ground;
                }
                _unitTypeEnum = parsed;
            }
            return _unitTypeEnum.Value;
        }
    }
    #endregion

    #region Attack Mode caching
    [NonSerialized]
    private AttackMode? _attackModeEnum;
    public AttackMode AttackModeEnum
    {
        get
        {
            if (_attackModeEnum == null)
            {
                if (!Enum.TryParse(attackMode, true, out AttackMode parsed))
                {
                    parsed = AttackMode.Melee;
                }
                _attackModeEnum = parsed;
            }
            return _attackModeEnum.Value;
        }
    }
    #endregion

    #region Attack target type caching
    [NonSerialized]
    private AttackTargetType? _attackTargetEnum;
    public AttackTargetType AttackTargetEnum
    {
        get
        {
            if (_attackTargetEnum == null)
            {
                if (!Enum.TryParse(attackTarget, true, out AttackTargetType parsed))
                {
                    parsed = AttackTargetType.GroundOnly;
                }
                _attackTargetEnum = parsed;
            }
            return _attackTargetEnum.Value;
        }
    }
    #endregion

    #region Rarity Enum caching
    [NonSerialized]
    private UnitRarity? _rarityEnum;
    public UnitRarity RarityEnum
    {
        get
        {
            if (_rarityEnum == null)
            {
                if (!Enum.TryParse(rarity, true, out UnitRarity parsed))
                {
                    parsed = UnitRarity.Common;
                }
                _rarityEnum = parsed;
            }
            return _rarityEnum.Value;
        }
    }
    #endregion
}
