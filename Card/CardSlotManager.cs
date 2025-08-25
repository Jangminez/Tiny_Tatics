using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSlotManager : MonoBehaviour
{
    [Header("소환 가능 카드 슬롯")]
    public CardDragHandler[] cardSlots;

    [Header("다음 생성 카드 슬롯")]
    public CardDragHandler nextCardSlot;

    private PlayerDataManager playerDataManager;
    private string nextCardId = string.Empty;

    void Awake()
    {
        // PlayerDataManager 참조 및 업그레이드 이벤트 구독
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        if (playerDataManager != null)
            playerDataManager.OnCardUpgraded += OnCardUpgraded;
    }

    void Start()
    {
        // 슬롯 등장 애니메이션
        AnimateCardSlots();

        // 다음 카드 미리 결정
        nextCardId = GetRandomCardId();

        // 초기 슬롯 세팅 및 사용 이벤트 연결
        for (int i = 0; i < cardSlots.Length; i++)
        {
            int slotIdx = i;
            ReplaceCard(slotIdx, false);
            cardSlots[slotIdx].OnCardUsed += () => ReplaceCard(slotIdx, true);
        }

        // 다음 카드 프리뷰 업데이트
        if (nextCardSlot != null)
            nextCardSlot.SetCard(nextCardId);

        // 마나 변화 구독 및 초기 갱신
        if (ManaSystem.Instance != null)
            ManaSystem.Instance.OnManaChanged += UpdateCardManaState;
        UpdateCardManaState();

        // 모든 슬롯의 별 레벨 강제 반영
        RefreshAllCardStars();
    }

    void OnDestroy()
    {
        // 이벤트 핸들러 해제
        if (playerDataManager != null)
            playerDataManager.OnCardUpgraded -= OnCardUpgraded;
        if (ManaSystem.Instance != null)
            ManaSystem.Instance.OnManaChanged -= UpdateCardManaState;
    }

    private void OnCardUpgraded(CardData card)
    {
        // 카드 업그레이드 시 별 레벨 갱신
        RefreshAllCardStars();
    }

    /// <summary>
    /// 화면에 표시된 모든 슬롯의 레벨 표시를 갱신합니다.
    /// </summary>
    private void RefreshAllCardStars()
    {
        foreach (var slot in cardSlots)
            slot.SetCard(slot.cardId);

        if (nextCardSlot != null)
            nextCardSlot.SetCard(nextCardId);
    }

    void AnimateCardSlots()
    {
        float delayStep = 0.08f;
        float duration = 0.28f;

        for (int i = 0; i < cardSlots.Length; i++)
        {
            var t = cardSlots[i].transform;
            t.localScale = Vector3.zero;
            t.DOScale(Vector3.one, duration)
             .SetEase(Ease.OutBack)
             .SetDelay(i * delayStep);
        }

        if (nextCardSlot != null)
        {
            var t = nextCardSlot.transform;
            t.localScale = Vector3.zero;
            t.DOScale(Vector3.one, duration)
             .SetEase(Ease.OutBack)
             .SetDelay(cardSlots.Length * delayStep);
        }
    }

    private void UpdateCardManaState()
    {
        float currentMana = ManaSystem.Instance.GetCurrentMana();

        // SpawnCardSlot을 통한 마나 상태 업데이트
        foreach (var slot in cardSlots)
        {
            var spawnSlot = slot.GetComponent<SpawnCardSlot>();
            if (spawnSlot != null)
                spawnSlot.UpdateManaState(currentMana);
        }
        if (nextCardSlot != null)
        {
            var spawnPreview = nextCardSlot.GetComponent<SpawnCardSlot>();
            if (spawnPreview != null)
                spawnPreview.UpdateManaState(currentMana);
        }
    }

    /// <summary>
    /// slotIdx의 카드를 교체합니다.
    /// useNextCard=false: 초기 랜덤, true: 다음 카드 사용
    /// </summary>
    public void ReplaceCard(int slotIdx, bool useNextCard)
    {
        if (playerDataManager == null)
            playerDataManager = FindObjectOfType<PlayerDataManager>();

        var list = playerDataManager.OwnedcardIds;
        if (list == null || list.Count == 0)
            return;

        // 카드 선택
        string cardIdToSet = useNextCard ? nextCardId : GetRandomCardId();
        if (useNextCard)
            nextCardId = GetRandomCardId();

        // CardDragHandler 업데이트 및 SpawnCardSlot 세팅
        cardSlots[slotIdx].SetCard(cardIdToSet);
        var spawnSlot = cardSlots[slotIdx].GetComponent<SpawnCardSlot>();
        if (spawnSlot != null)
            spawnSlot.SetCard(cardIdToSet);

        // 다음 카드 프리뷰 업데이트
        if (nextCardSlot != null)
            nextCardSlot.SetCard(nextCardId);
    }

    /// <summary>
    /// 모든 슬롯 비우고 재생성 + 애니메이션
    /// </summary>
    public void ResetAllSlots()
    {
        StartCoroutine(RegenAllSlotsWithAnimation());
    }

    private IEnumerator RegenAllSlotsWithAnimation()
    {
        float delayStep = 0.08f;
        float duration = 0.28f;

        // 다음 카드 결정
        nextCardId = GetRandomCardId();

        // 슬롯별 재세팅 및 애니메이션 수행
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].SetCard(GetRandomCardId());
            cardSlots[i].gameObject.SetActive(true);

            var t = cardSlots[i].transform;
            t.localScale = Vector3.zero;
            t.DOScale(Vector3.one, duration)
             .SetEase(Ease.OutBack)
             .SetDelay(0);

            yield return new WaitForSeconds(delayStep);
        }

        if (nextCardSlot != null)
        {
            nextCardSlot.SetCard(nextCardId);
            nextCardSlot.gameObject.SetActive(true);

            var t = nextCardSlot.transform;
            t.localScale = Vector3.zero;
            t.DOScale(Vector3.one, duration)
             .SetEase(Ease.OutBack)
             .SetDelay(0);
        }

        // 마나 상태 & 별 레벨 갱신
        UpdateCardManaState();
        RefreshAllCardStars();
    }

    private string GetRandomCardId()
    {
        var list = playerDataManager.OwnedcardIds;
        return (list != null && list.Count > 0)
            ? list[Random.Range(0, list.Count)]
            : string.Empty;
    }
}
