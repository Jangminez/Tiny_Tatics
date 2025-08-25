using UnityEngine;
using UnityEngine.UI;

public class StoreFixed : BaseFixed
{
    [SerializeField]
    private Button buyButton;

    [SerializeField]
    private Button playerButton;

    private PlayerDataManager playerData;

    private UIManager uiManager;
    public override UIType UIType => UIType.StoreFixed;

    public override void OnOpen()
    {
        PlayClickSound();
        uiManager = UIManager.Instance;
        playerData = GameManager.Instance.PlayerDataManager;

        buyButton.onClick.AddListener(OnClickBuyButton);
        playerButton.onClick.AddListener(OnClickPlayerButton);
    }

    public override void OnClose()
    {
        buyButton.onClick.RemoveListener(OnClickBuyButton);
        playerButton.onClick.RemoveListener(OnClickPlayerButton);
    }

    public void OnClickBuyButton()
    {
        PlayClickSound();

        if (playerData.selectedCard != null)
        {
            ShowConfirmPopup(
                "카드 구매",
                $"'{playerData.selectedCard.cardName}' 카드를 구매하시겠습니까?",
                playerData.selectedCard.cardId,
                () => playerData.AddCard(playerData.selectedCard.cardId)
            );
        }
        else if (playerData.selectedItem != null)
        {
            ShowConfirmPopup(
                "아이템 구매",
                $"'{playerData.selectedItem.relicName}' 아이템을 구매하시겠습니까?",
                playerData.selectedItem.relicId,
                () => playerData.AddItem(playerData.selectedItem.relicId)
            );
        }
    }

    public void OnClickPlayerButton()
    {
        PlayClickSound();
        uiManager.OpenWindow(UIType.StageMapWindow);
    }

    private void ShowConfirmPopup(string title, string message, string id, System.Action onConfirm)
    {
        var param = new ConfirmPopupParam
        {
            Title = title,
            Message = message,
            TargetId = id,
            OnConfirm = onConfirm,
        };
        uiManager.OpenPopup(UIType.ConfirmPopup, param);
    }

    private void PlayClickSound()
    {
        SoundManager.Instance.SetSFXVolume(0.5f);
        SoundManager.Instance.PlaySFX("Click");
    }
}
