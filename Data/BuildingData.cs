using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public string buildingId;
    public string buildingName;
    public string description;
    public string iconPath;
    public string prefabPath;

    public float baseHp;
    public float baseAttackDamage;
    public float baseAttackSpeed;
    public float baseAttackRange;

    public int maxLevel;

    public float hpPerLevel;
    public float attackDamagePerLevel;
    public float attackSpeedPerLevel;
    public float attackRangePerLevel;

    public float GetHpAtLevel(int level) =>
        baseHp + hpPerLevel * (level - 1);

    public float GetAttackDamageAtLevel(int level) =>
        baseAttackDamage + attackDamagePerLevel * (level - 1);

    public float GetAttackSpeedAtLevel(int level) =>
        baseAttackSpeed + attackSpeedPerLevel * (level - 1);

    public float GetAttackRangeAtLevel(int level) =>
        baseAttackRange + attackRangePerLevel * (level - 1);
}
