using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCardSlot : MonoBehaviour
{
    public Image costBgImage;
    public TextMeshProUGUI costText;

    // public Image iconImage; [삭제] 이미지 직접 할당 안함
    public Image cooldownImage;

    [SerializeField] private GameObject blockRay;
    
    [HideInInspector]
    public string cardId;

    private int manaCost;
    private float cooldownTime;
    private Button button;
    private float cooldownTimer = 0f;
    private bool isCoolDown = false;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f;
    }

    public void SetCard(string id)
    {
        cardId = id;
        var card = GameManager.Instance.GameDataManager.GetCardById(cardId);
        if (card != null)
        {
            Debug.Log($"SetCard: {cardId} / {card.cardName} / cost={card.cost}");
            manaCost = card.cost;
            cooldownTime = card.spawnDelay > 0 ? card.spawnDelay : 5f;
            if (costText != null)
                costText.text = manaCost.ToString();
            else
                Debug.LogError($"{gameObject.name} - costText가 연결 안됨!");
            if (costBgImage != null)
                costBgImage.color = Color.white;
            else
                Debug.LogError($"{gameObject.name} - costBgImage가 연결 안됨!");
        }
        else
        {
            if (costText != null)
                costText.text = "-";
            if (costBgImage != null)
                costBgImage.color = Color.gray;
        }

        // 이미지 관련해서는 CardDragHandler에서 할당 
        //var card = CardDB.Instance.GetCard(cardId);
        //if (card == null)
        //{
        //    Debug.LogError($"CardData not found for cardId: {cardId}");
        //    iconImage.sprite = null;
        //    return;
        //}

        //var sprite = CardDB.Instance.GetCardIcon(cardId);
        //if (sprite == null)
        //    Debug.LogError($"Card Icon not found for iconPath: {card.iconPath}");
        //iconImage.sprite = sprite;

        //manaCost = card.cost;
        //cooldownTime = card.spawnDelay > 0 ? card.spawnDelay : 5f;

    }

    void Update()
    {
        if (isCoolDown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                isCoolDown = false;
                cooldownImage.fillAmount = 0;
                button.interactable = true;
            }
            else
            {
                cooldownImage.fillAmount = cooldownTimer / cooldownTime;
            }
        }
    }

    private void OnClick()
    {
        // 카드 소환은 드래그로만 가능하게끔 수정 
        if (isCoolDown)
            return;

        // 기존 코드 
        //if (isCoolDown)
        //    return;
        //var battleFixed = UIManager.Instance.GetFixed<BattleFixed>();
        //if (battleFixed != null && battleFixed.UseMana(manaCost))
        //{
        //    Debug.Log($"카드 사용, 마나 {manaCost} 소비됨");
        //    StartCooldown();
        //}
        //else
        //{
        //    Debug.Log($"마나 부족, (필요: {manaCost})");
        //}
    }
    
    public void UpdateManaState(float currentMana)
    {
        bool isEnough = currentMana >= manaCost;
        
        if (blockRay != null)
            blockRay.SetActive(!isEnough); // 부족하면 회색 보이게

        if (button != null && !isCoolDown)
            button.interactable = isEnough;
    }


    private void StartCooldown()
    {
        cooldownTimer = cooldownTime;
        isCoolDown = true;
        cooldownImage.fillAmount = 1f;
        button.interactable = false;
    }
    public int GetManaCost() => manaCost;
    public float GetCooldownTime() => cooldownTime;
    public bool IsCooldown() => isCoolDown;
}
