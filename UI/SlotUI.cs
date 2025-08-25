using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Button buyButton;

    private Action onClick;

    public void Init(CardData card, Action onSelected)
    {
        icon.sprite = Resources.Load<Sprite>(card.iconPath);
        onClick = onSelected;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => onClick?.Invoke());
    }

    public void Init(RelicData relic, Action onSelected)
    {
        icon.sprite = Resources.Load<Sprite>(relic.iconPath);
        onClick = onSelected;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => onClick?.Invoke());
    }
}
