using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearWindow : BaseWindow
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Image cardIcon;

    [SerializeField]
    private Image goldIcon;

    [SerializeField]
    private Image levelUpIcon;

    [SerializeField]
    private Image increaseIcon;
    private PlayerDataManager playerData;
    private GameDataManager gameData;
    private int additionalGold = 300;
    private bool isCardLevelUp;
    public override UIType UIType => UIType.StageClearWindow;

    public override void OnOpen()
    {
        UIEffectHelper.PlayPopupOpen(transform);

        playerData = GameManager.Instance.PlayerDataManager;
        gameData = GameManager.Instance.GameDataManager;

        playButton.onClick.AddListener(OnClickPlay);
        TryLevelupRandomCard();
        UpdateUI();

        playButton.interactable = true;
    }

    public override void OnClose()
    {
        playButton.onClick.RemoveListener(OnClickPlay);
    }

    private void OnClickPlay()
    {
        playButton.interactable = false;

        UIEffectHelper.PlayPopupClose(transform, () =>
        {
            UIManager.Instance.CloseWindow(UIType.StageClearWindow);
            UIManager.Instance.OpenWindow(UIType.StageMapWindow);
            Time.timeScale = 1;
        });
    }

    private void UpdateUI()
    {
        if (isCardLevelUp)
        {
            cardIcon.gameObject.SetActive(true);
            levelUpIcon.gameObject.SetActive(true);
            goldIcon.gameObject.SetActive(false);
            increaseIcon.gameObject.SetActive(false);
        }
        else
        {
            cardIcon.gameObject.SetActive(false);
            levelUpIcon.gameObject.SetActive(false);
            goldIcon.gameObject.SetActive(true);
            increaseIcon.gameObject.SetActive(true);
        }
    }

    private void TryLevelupRandomCard()
    {
        List<CardData> lowLevelCards = GetLowLevelCard();

        if (lowLevelCards.Count == 0)
        {
            AddGold();
            isCardLevelUp = false;
        }
        else
        {
            int idx = Random.Range(0, lowLevelCards.Count);
            CardData selectedCard = lowLevelCards[idx];
            LevelupCard(selectedCard);
            isCardLevelUp = true;
        }
    }

    private List<CardData> GetLowLevelCard()
    {
        var result = new List<CardData>();
        foreach (var cardId in playerData.OwnedcardIds)
        {
            var card = gameData.GetCardById(cardId);
            if (playerData.GetCardLevel(card.unitId) < 5)
                result.Add(card);
        }
        return result;
    }

    private void LevelupCard(CardData card)
    {
        int level = playerData.GetCardLevel(card.unitId);
        playerData.SetCardLevel(card.unitId, level + 1);
        cardIcon.sprite = Resources.Load<Sprite>(card.iconPath);
    }

    private void AddGold()
    {
        playerData.AddGold(additionalGold);
        UIManager.Instance.GetWindow<GoldWindow>().Refresh();
    }
}
