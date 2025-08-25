using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameManager gameManager;

    public string cardId;
    private CardData cardData;

    [Header("카드 이미지 오브젝트 할당")]
    public Image cardImage;

    [Header("별 이미지 오브젝트 배열")]
    public Image[] starImages;

    public System.Action OnCardUsed; // 소환 성공 시 호출할 델리게이트

    private Canvas canvas;
    private GameObject previewObj;
    private Vector3 lastWorldPos;

    public void SetCard(string newCardId)
    {
        if(gameManager == null)
            gameManager = GameManager.Instance;

        cardId = newCardId;
        cardData = gameManager.GameDataManager.GetCardById(cardId);

        // 카드 이미지 자동 세팅
        if (cardImage != null && cardData != null && !string.IsNullOrEmpty(cardData.iconPath))
        {
            Sprite sprite = Resources.Load<Sprite>(cardData.iconPath);
            if (sprite != null)
                cardImage.sprite = sprite;
        }

        // 카드 레벨 정보 표시
        if (starImages != null)
        {
            int level = 1;

            var playerData = FindObjectOfType<PlayerDataManager>();
            if (cardData != null && playerData != null && !string.IsNullOrEmpty(cardData.unitId))
            {
                level = playerData.GetCardLevel(cardData.unitId);
            }
            // 없는 카드이거나 null이면 별 모두 끄기
            for (int i = 0; i < starImages.Length; i++)
            {
                starImages[i].gameObject.SetActive(cardData != null && i < level);
            }
        }

        // 슬롯의 SpawnCardSlot에도 코스트 등 자동 갱신
        var spawnCardSlot = GetComponent<SpawnCardSlot>();
        if (spawnCardSlot != null)
            spawnCardSlot.SetCard(cardId);
    }

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 마나 부족 시 드래그 불가 (프리뷰 생성 안함)
        if (cardData == null || ManaSystem.Instance.GetCurrentMana() < cardData.cost)
        {
            // TODO: 효과음/진동/색상 등 추가 피드백 가능
            eventData.pointerDrag = null; // 드래그 중단
            return;
        }

        SpawnRangeVisualizer.Instance?.Show();

        // 드래그 프리뷰 유닛 생성 
        if (cardData != null && cardData.spawnUnits != null && cardData.spawnUnits.Count > 0)
        {
            var prefab = Resources.Load<GameObject>(cardData.spawnUnits[0].unitPrefabPath);
            if (prefab)
            {
                previewObj = GameObject.Instantiate(prefab);
                previewObj.transform.rotation = Quaternion.Euler(0, -90, 0);
                SetPreviewTransparent(previewObj, 0.6f);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (previewObj)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out var hit, 100f, LayerMask.GetMask("Ground")))
            {
                var pos = hit.point;
                float minX = SpawnRangeVisualizer.Instance.minX;
                float maxX = SpawnRangeVisualizer.Instance.maxX;
                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                previewObj.transform.position = pos;
                previewObj.transform.rotation = Quaternion.Euler(0, -90, 0);
                lastWorldPos = pos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SpawnRangeVisualizer.Instance?.Hide();

        if (previewObj)
        {
            Destroy(previewObj);
            previewObj = null;
        }

        if (cardData == null) return;

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out var hit, 100f, LayerMask.GetMask("Ground")))
        {
            var pos = hit.point;
            float minX = SpawnRangeVisualizer.Instance.minX;
            float maxX = SpawnRangeVisualizer.Instance.maxX;
            if (pos.x < minX || pos.x > maxX)
                return; // 소환불가

            if (ManaSystem.Instance.ConsumeMana(cardData.cost))
            {
                CharacterSpawner.Instance.SpawnByCardData(cardData, pos);

                // 여기서 카드 "사용됨" 처리
                OnCardUsed?.Invoke();
            }
        }
    }

    void SetPreviewTransparent(GameObject obj, float alpha)
    {
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                mat.SetFloat("_Surface", 1); // transparent
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }
}
