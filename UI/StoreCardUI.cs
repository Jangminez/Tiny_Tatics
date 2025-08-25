using System.Linq;
using UnityEngine;

public class StoreCardUI : MonoBehaviour
{
    [SerializeField]
    private Transform cardSlotParent;

    [SerializeField]
    private GameObject cardSlotPrefab;

    [SerializeField]
    private StorePreviewUI previewPanel;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        var unowned = GameManager.Instance.GameDataManager
            .AllCards.Where(c =>
                !GameManager.Instance.PlayerDataManager.IsOwnedCard(c.cardId)
            )
            .OrderBy(_ => Random.value)
            .Take(3)
            .ToList();

        foreach (Transform child in cardSlotParent)
            Destroy(child.gameObject);

        foreach (var card in unowned)
        {
            var go = Instantiate(cardSlotPrefab, cardSlotParent);
            var ui = go.GetComponent<SlotUI>();
            ui.Init(card, () => OnCardSelected(card));
        }
    }

    private void OnCardSelected(CardData card)
    {
        GameManager.Instance.PlayerDataManager.SelectCard(card);
        previewPanel.ShowCardPreview(card);
    }
}
