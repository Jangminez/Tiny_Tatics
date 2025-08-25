using TMPro;
using UnityEngine;

public class GoldWindow : BaseWindow
{
    [SerializeField]
    private TextMeshProUGUI goldText;
    public override UIType UIType => UIType.GoldWindow;
    private PlayerDataManager playerData;

    public override void OnOpen()
    {
        playerData = GameManager.Instance.PlayerDataManager;

        goldText.text = playerData.GetGold().ToString();
    }

    public void Refresh()
    {
        goldText.text = playerData.GetGold().ToString();
    }
}
