using System.Linq;
using UnityEngine;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField]
    private Transform cardSlotParent;

    [SerializeField]
    private GameObject cardSlotPrefab;

    [SerializeField]
    private UpgradePreviewUI previewUI;

    [SerializeField]
    private GameObject previewPanel;

    private PlayerDataManager playerData;
    private GameDataManager gameData;

    void Start()
    {
        playerData = GameManager.Instance.PlayerDataManager;
        gameData = GameManager.Instance.GameDataManager;
        Refresh();
        previewPanel.SetActive(false);
    }

    public void Refresh()
    {
        var uniqueCardIds = playerData.OwnedcardIds.Distinct();

        var ownedCards = uniqueCardIds
            .Select(id => gameData.GetCardById(id))
            .Where(card => card != null)
            .ToList();

        foreach (Transform child in cardSlotParent)
            Destroy(child.gameObject);

        foreach (var card in ownedCards)
        {
            var go = Instantiate(cardSlotPrefab, cardSlotParent);
            var ui = go.GetComponent<SlotUI>();
            ui.Init(card, () => OnCardSelected(card));
        }
    }

    private void OnCardSelected(CardData card)
    {
        playerData.SelectCard(card);
        previewUI.ShowCardPreview(card);
        previewPanel.SetActive(true);
    }
}
