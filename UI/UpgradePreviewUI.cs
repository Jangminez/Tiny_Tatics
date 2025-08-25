using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePreviewUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI beforeAtkText;

    [SerializeField]
    private TextMeshProUGUI beforeMaxHPText;

    [SerializeField]
    private TextMeshProUGUI afterAtkText;

    [SerializeField]
    private TextMeshProUGUI afterMaxHPText;

    [SerializeField]
    private Image beforeIcon;

    [SerializeField]
    private Image afterIcon;

    [SerializeField]
    private GameObject upgradeAfterPanel;

    [SerializeField]
    private GameObject maxLevelMessage;

    [SerializeField]
    private Image GoldIcon;

    [SerializeField]
    private Image[] beforeStarIcon;

    [SerializeField]
    private Image[] afterStarIcon;
    private PlayerDataManager playerData;
    private GameDataManager gameData;

    private void Start()
    {
        playerData = GameManager.Instance.PlayerDataManager;
        gameData = GameManager.Instance.GameDataManager;
        
        GoldIcon.gameObject.SetActive(false);
        upgradeAfterPanel.SetActive(false);
        maxLevelMessage.SetActive(false);
        playerData.OnCardUpgraded += ShowCardPreview;
    }

    public void ShowCardPreview(CardData card)
    {
        var unit = gameData.GetUnitById(card.unitId);
        var statBefore = new UnitStat(unit);
        var statAfter = new UnitStat(unit);
        int level = playerData.GetCardLevel(card.unitId);
        int price = card.price * level;
        var icon = Resources.Load<Sprite>(card.iconPath);

        beforeIcon.sprite = icon;
        afterIcon.sprite = icon;

        priceText.text = price.ToString();
        GoldIcon.gameObject.SetActive(true);
        gameObject.SetActive(true);

        UpgradeBeforeCard(unit, statBefore, level);

        if (level >= 5)
        {
            upgradeAfterPanel.SetActive(false);
            maxLevelMessage.SetActive(true);
        }
        else
        {
            UpgradeAfterCard(unit, statAfter, level);
            upgradeAfterPanel.SetActive(true);
            maxLevelMessage.SetActive(false);
        }
    }

    private void UpgradeBeforeCard(UnitData unit, UnitStat stat, int level)
    {
        int cardLevel = ApplyCardUpgradeBonus(stat, level, true);

        nameText.text = unit.unitName;
        beforeAtkText.text = $"공격력 : {stat.AttackDamage}";
        beforeMaxHPText.text = $"최대체력 : {stat.MaxHP}";
        for (int i = 0; i < afterStarIcon.Length; i++)
        {
            beforeStarIcon[i].gameObject.SetActive(i < cardLevel);
        }
    }

    private void UpgradeAfterCard(UnitData unit, UnitStat stat, int level)
    {
        int cardLevel = ApplyCardUpgradeBonus(stat, level, false);

        nameText.text = unit.unitName;
        afterAtkText.text = $"공격력 : {stat.AttackDamage}";
        afterMaxHPText.text = $"최대체력 : {stat.MaxHP}";
        for (int i = 0; i < afterStarIcon.Length; i++)
        {
            afterStarIcon[i].gameObject.SetActive(i < cardLevel + 1);
        }
    }

    private int ApplyCardUpgradeBonus(UnitStat stat, int level, bool isBeforeUpgrade)
    {
        if (level > 5)
            level = 5;

        int val = isBeforeUpgrade ? level - 1 : level;

        stat.MaxHP = Mathf.RoundToInt(stat.MaxHP * (1f + 0.2f * val));
        stat.AttackDamage = Mathf.RoundToInt(stat.AttackDamage * (1f + 0.2f * val));
        return level;
    }

    public void HidePreview()
    {
        gameObject.SetActive(false);
        playerData.ClearSelection();
    }
}
