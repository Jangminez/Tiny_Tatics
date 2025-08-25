using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public CardData selectedCard { get; set; }
    public RelicData selectedItem { get; set; }

    [SerializeField]
    private List<string> defaultCardIds = new();

    [SerializeField]
    private List<string> ownedCardIds = new();

    [SerializeField]
    private List<string> ownedItemIds = new();

    public List<RelicData> unitItems = new();
    public List<RelicData> playerItems = new();

    public List<RelicData> consumableItems = new();

    private Dictionary<string, int> cardLevels = new();

    public List<string> OwnedcardIds => ownedCardIds;

    public event Action<CardData> OnCardUpgraded;
    public event Action OnCardPurchased;

    public int Gold;
    private bool isInitialized = false;

    public void Init()
    {
        if (isInitialized) return;
        isInitialized = true;

        ownedCardIds.Clear();
        unitItems.Clear();
        playerItems.Clear();
        consumableItems.Clear();
        Gold = 0;

        foreach (var cardId in defaultCardIds)
        {     
            ownedCardIds.Add(cardId);
        }

        LoadCardLevel();
    }

    public void SelectCard(CardData card)
    {
        selectedCard = card;
        selectedItem = null;
    }

    public void SelectItem(RelicData item)
    {
        selectedItem = item;
        selectedCard = null;
    }

    public void ClearSelection()
    {
        selectedCard = null;
        selectedItem = null;
    }

    public bool IsOwnedCard(string cardId) => ownedCardIds.Contains(cardId);

    public bool IsOwnedItem(string itemId) => ownedItemIds.Contains(itemId);

    public void AddCard(string cardId)
    {
        if (Gold < selectedCard.price)
        {
            ShowAlert("골드가 부족합니다");
            return;
        }

        Gold -= selectedCard.price;
        ownedCardIds.Add(cardId);
        OnCardPurchased?.Invoke();

        ShowAlert("구매했습니다");
        UIManager.Instance.GetWindow<GoldWindow>().Refresh();
    }

    public void AddItem(string itemId)
    {
        if (Gold < selectedItem.price)
        {
            ShowAlert("골드가 부족합니다");
            return;
        }

        if (ownedItemIds.Contains(itemId))
        {
            ShowAlert("보유중인\n아이템입니다.");
            return;
        }

        Gold -= selectedItem.price;
        ownedItemIds.Add(itemId);
        SplitItemType(selectedItem);
        ShowAlert("구매했습니다");
        UIManager.Instance.GetWindow<GoldWindow>().Refresh();
    }

    private void ShowAlert(string message)
    {
        UIManager.Instance.CloseTopPopup();
        UIManager.Instance.OpenPopup(UIType.AlertPopup, new AlertPopupParam { Message = message });
    }

    public void SplitItemType(RelicData item)
    {
        switch (item.relicType)
        {
            case "Passive":
                if (item.targetType == "Unit")
                    unitItems.Add(item);
                else if (item.targetType == "Player")
                    playerItems.Add(item);
                break;
            case "Consumable":
                consumableItems.Add(item);
                break;
            case "Conditional":
                // 나중에 생기면 추가
                break;
        }
    }

    public int GetCardLevel(string unitId)
    {
        if (cardLevels.TryGetValue(unitId, out var level))
            return level;

        return 1;
    }

    public void SetCardLevel(string unitId, int level)
    {
        if (level > 5)
            return;
        cardLevels[unitId] = level;
        // 카드레벨 저장 추가 예쩡
    }

    public void LevelUpCard(string unitId)
    {
        int level = GetCardLevel(unitId);
        int price = selectedCard.price * level;

        if (level >= 5)
        {
            ShowAlert("최대 강화 수치입니다");
            return;
        }

        if (Gold < price)
        {
            ShowAlert("골드가 부족합니다");
            return;
        }

        Gold -= price;
        SetCardLevel(unitId, level + 1);
        OnCardUpgraded?.Invoke(selectedCard);
        ShowAlert("구매했습니다");
        UIManager.Instance.GetWindow<GoldWindow>().Refresh();
    }

    public void AddGold(int gold)
    {
        Gold += gold;
        UIManager.Instance.GetWindow<GoldWindow>().Refresh();
    }

    public int GetGold()
    {
        return Gold;
    }

    private void LoadCardLevel()
    {
        // 카드레벨 불러오기 추가 예정
    }
}
