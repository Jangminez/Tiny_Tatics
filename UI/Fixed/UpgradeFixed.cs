using UnityEngine;
using UnityEngine.UI;

public class UpgradeFixed : BaseFixed
{
    public Button buyButton;
    public Button playerButton;
    public override UIType UIType => UIType.UpgradeFixed;
    private PlayerDataManager playerData;

    public override void OnOpen()
    {
        playerData = GameManager.Instance.PlayerDataManager;

        buyButton.onClick.AddListener(OnClickUpgradeButton);
        playerButton.onClick.AddListener(OnClickPlayButton);
    }

    public override void OnClose()
    {
        buyButton.onClick.RemoveListener(OnClickUpgradeButton);
        playerButton.onClick.RemoveListener(OnClickUpgradeButton);
    }

    public void OnClickUpgradeButton()
    {
        PlayClickSound();
        if (playerData.selectedCard != null)
        {
            var param = new ConfirmPopupParam
            {
                Title = "카드 업그레이드",
                Message = $"'{playerData.selectedCard.cardName}' 카드를 구매하시겠습니까?",
                TargetId = playerData.selectedCard.cardId,
                OnConfirm = () =>
                {
                    playerData.LevelUpCard(playerData.selectedCard.unitId);
                },
            };
            UIManager.Instance.OpenPopup(UIType.ConfirmPopup, param);
        }
    }

    public void OnClickPlayButton()
    {
        PlayClickSound();
        UIManager.Instance.OpenWindow(UIType.StageMapWindow);
    }

    private void PlayClickSound()
    {
        SoundManager.Instance.SetSFXVolume(0.5f);
        SoundManager.Instance.PlaySFX("Click");
    }
}
