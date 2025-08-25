using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageNodeButton : MonoBehaviour
{
    [SerializeField] Button nodeButton;
    [SerializeField] Image nodeIcon;

    private StageNodeData nodeData;

    public void Init(StageNodeData data, Sprite icon, Action<StageNodeData> onClicked)
    {
        nodeData = data;
        nodeIcon.sprite = icon;
        nodeButton.onClick.AddListener(() => onClicked?.Invoke(nodeData));
    }

    public void SetInteractable(bool canInteract)
    {
        nodeButton.interactable = canInteract;
    }

    public string GetNodeID() => nodeData.nodeID;
}
