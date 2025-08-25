using System.Linq;
using UnityEngine;

public class StoreItemUI : MonoBehaviour
{
    [SerializeField]
    private Transform relicSlotParent;

    [SerializeField]
    private GameObject relicSlotPrefab;

    [SerializeField]
    private StorePreviewUI previewPanel;

    private PlayerDataManager playerData;
    private GameDataManager gameData;
    void Start()
    {
        playerData = GameManager.Instance.PlayerDataManager;
        gameData = GameManager.Instance.GameDataManager;
        Refresh();
    }

    public void Refresh()
    {
        var unowned = gameData
            .AllRelics.Where(c => !playerData.IsOwnedItem(c.relicId))
            .OrderBy(_ => Random.value)
            .Take(3)
            .ToList();

        foreach (Transform child in relicSlotParent)
            Destroy(child.gameObject);

        foreach (var relic in unowned)
        {
            var go = Instantiate(relicSlotPrefab, relicSlotParent);
            var ui = go.GetComponent<SlotUI>();
            ui.Init(relic, () => OnItemSelected(relic));
        }
    }

    private void OnItemSelected(RelicData item)
    {
        playerData.SelectItem(item);
        previewPanel.ShowItemPreview(item);
    }
}
