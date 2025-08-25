using UnityEngine;

public class UnitStatManager : MonoBehaviour
{
    private PlayerDataManager playerData;

    public bool cheatMode;

    public void Init(GameManager gameManager)
    {
        playerData = gameManager.PlayerDataManager;
    }

    public UnitStat GetFinalStat(UnitData unit)
    {
        UnitStat stat = new UnitStat(unit);

        foreach (var item in playerData.unitItems)
        {
            bool isMatch = IsApplicableToUnit(item, unit);

            if (!isMatch)
                continue;

            ApplyItemBonus(stat, item);
        }
        ApplyCardUpgradeBonus(stat, unit.unitId);

        if (cheatMode)
        {
            ApplyCheatBonus(stat);
            playerData.Gold = 9999;
        }

        return stat;
    }

    private void ApplyCheatBonus(UnitStat stat)
    {
        stat.MaxHP *= 9999f;
        stat.AttackDamage *= 9999f;
        stat.MoveSpeed *= 5f;
        stat.AttackInterval *= 0.01f;
    }

    private void ApplyItemBonus(UnitStat stat, RelicData item)
    {
        switch (item.effectType)
        {
            case "AttackDamage":
                stat.AttackDamage = Mathf.RoundToInt(stat.AttackDamage * (1 + item.effectValue));
                break;

            case "MoveSpeed":
                stat.MoveSpeed *= 1 + item.effectValue;
                break;

            case "AttackSpeed":
                stat.AttackInterval *= 1 - item.effectValue;
                break;
        }
    }

    private void ApplyCardUpgradeBonus(UnitStat stat, string unitId)
    {
        int level = playerData.GetCardLevel(unitId);
        if (level <= 5)
        {
            stat.MaxHP = Mathf.RoundToInt(stat.MaxHP * (1f + 0.2f * (level - 1)));
            stat.AttackDamage = Mathf.RoundToInt(stat.AttackDamage * (1f + 0.2f * (level - 1)));
        }
    }

    private bool IsApplicableToUnit(RelicData item, UnitData unit)
    {
        if (item.targetType != "Unit")
            return false;

        switch (item.effectTarget)
        {
            case "All":
                return true;
            case "Melee":
                return unit.AttackModeEnum == AttackMode.Melee;
            case "Ranged":
                return unit.AttackModeEnum == AttackMode.Ranged;
            default:
                return false;
        }
    }
}

public class UnitStat
{
    public float MaxHP { get; set; }
    public float CurrentHP { get; set; }
    public float AttackDamage { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackRange { get; set; }
    public float AttackInterval { get; set; }

    public UnitStat(UnitData data)
    {
        MaxHP = data.hp;
        CurrentHP = data.hp;
        AttackDamage = data.attackDamage;
        MoveSpeed = data.moveSpeed;
        AttackRange = data.attackRange;
        AttackInterval = data.attackInterval;
    }
}

public class CardUpgradeData
{
    public string UnitId { get; set; }
    public int level { get; set; }

    public CardUpgradeData(CardData data)
    {
        UnitId = data.unitId;
        level = 1;
    }
}
