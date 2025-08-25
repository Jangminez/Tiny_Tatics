using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorePreviewUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image GoldIcon;

    private void Start()
    {
        GoldIcon.gameObject.SetActive(false);
    }

    public void ShowCardPreview(CardData card)
    {
        ShowPreview(card.cardName, card.description, card.price, card.iconPath);
    }

    public void ShowItemPreview(RelicData item)
    {
        ShowPreview(item.relicName, item.description, item.price, item.iconPath);
    }

    private void ShowPreview(string name, string desc, int price, string iconPath)
    {
        priceText.text = price.ToString();
        nameText.text = name;
        descriptionText.text = desc;
        icon.sprite = Resources.Load<Sprite>(iconPath);
        GoldIcon.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void HidePreview()
    {
        gameObject.SetActive(false);
        GameManager.Instance.PlayerDataManager.ClearSelection();
    }
}
