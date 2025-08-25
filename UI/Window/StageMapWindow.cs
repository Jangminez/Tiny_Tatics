using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class StageMapWindow : BaseWindow
{
    private GameManager gameManager;

    [SerializeField]
    private RectTransform nodeParent;

    [SerializeField]
    private StageNodeButton nodePrefab;

    [SerializeField]
    private UILineConnector linePrefab;

    [SerializeField]
    Sprite[] nodeIcons;

    [SerializeField]
    private RectTransform cameraTargetRect;

    private Dictionary<string, StageNodeButton> nodeUIDict =
        new Dictionary<string, StageNodeButton>();

    public override UIType UIType => UIType.StageMapWindow;

    public override void OnOpen()
    {
        SoundManager.Instance.SetSFXVolume(0.5f);
        SoundManager.Instance.PlaySFX("Click");
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }

        if (gameManager.CurrentNode == null)
        {
            StartCoroutine(DrawMapAnimated());
        }
        else
        {
            SetInteractableNodes(gameManager.CurrentNode.connectedNodeIds);
        }
    }

    public override void OnClose() { }

    [ContextMenu("DrawMap")]
    private System.Collections.IEnumerator DrawMapAnimated()
    {
        StageMapData mapData = GameManager.Instance.StageDataManager.MapLoader.GetRandomMap();
        foreach (Transform child in nodeParent)
        {
            Destroy(child.gameObject);
        }

        nodeUIDict.Clear();

        float delayPerNode = 0.10f;
        float moveDuration = 0.2f;

        // 노드 y좌표 기준 오름차순 정렬(위->아래)
        var sortedNodes = mapData.nodes.OrderBy(n => n.uiPosition.y).ToList();

        for (int i = 0; i < sortedNodes.Count; i++)
        {
            var node = sortedNodes[i];
            var ui = Instantiate(nodePrefab, nodeParent);
            ui.Init(node, nodeIcons[(int)node.type], OnNodeClicked);

            var rt = ui.GetComponent<RectTransform>();
            rt.anchoredPosition = node.uiPosition;

            nodeUIDict[node.nodeID] = ui;

            // 처음엔 축소
            rt.localScale = Vector3.zero;
            rt.DOScale(Vector3.one, 0.33f).SetEase(Ease.OutBack);

            // 카메라(혹은 맵뷰) 따라오기
            if (cameraTargetRect != null)
            {
                // 생성되는 노드따라 이동 x값 고정
                Vector2 targetPos = cameraTargetRect.anchoredPosition;
                targetPos.y = rt.anchoredPosition.y;
                cameraTargetRect.DOAnchorPos(targetPos, moveDuration).SetEase(Ease.InOutSine);
            }

            yield return new WaitForSeconds(delayPerNode);
        }

        // 연결선은 노드 생성 이후 한번에 생성
        foreach (var node in mapData.nodes)
        {
            foreach (var nextId in node.connectedNodeIds)
            {
                if (nodeUIDict.TryGetValue(nextId, out var nextNode))
                {
                    var start = GetTopCenter(nodeUIDict[node.nodeID].GetComponent<RectTransform>());
                    var end = GetBottomCenter(nextNode.GetComponent<RectTransform>());

                    var line = Instantiate(linePrefab, nodeParent);
                    line.Connect(start, end);
                }
            }
        }

        ActivateStartNodes();
    }

    //public void DrawMap()
    //{
    //    StageMapData mapData = GameManager.Instance.StageDataManager.MapLoader.GetRandomMap();

    //    nodeUIDict.Clear();

    //    foreach (StageNodeData node in mapData.nodes)
    //    {
    //        var ui = Instantiate(nodePrefab, nodeParent);
    //        ui.Init(node, nodeIcons[(int)node.type], OnNodeClicked);

    //        var rt = ui.GetComponent<RectTransform>();
    //        rt.anchoredPosition = node.uiPosition;

    //        nodeUIDict[node.nodeID] = ui;
    //    }

    //    foreach (var node in mapData.nodes)
    //    {
    //        foreach (var nextId in node.connectedNodeIds)
    //        {
    //            if (nodeUIDict.TryGetValue(nextId, out var nextNode))
    //            {
    //                var start = GetTopCenter(nodeUIDict[node.nodeID].GetComponent<RectTransform>());
    //                var end = GetBottomCenter(nextNode.GetComponent<RectTransform>());

    //                var line = Instantiate(linePrefab, nodeParent);
    //                line.Connect(start, end);
    //            }
    //        }
    //    }

    //    ActivateStartNodes();
    //}

    // 선택 가능한 노드 활성화
    public void SetInteractableNodes(List<string> interactableNodeIDs)
    {
        foreach (string key in nodeUIDict.Keys)
        {
            bool canInteract = interactableNodeIDs.Contains(key);
            nodeUIDict[key].SetInteractable(canInteract);
        }
    }

    // 시작 노드들 활성화
    private void ActivateStartNodes()
    {
        var startNodes = nodeUIDict
            .Values.Where(ui => ui.GetNodeID().Contains("_1_"))
            .Select(ui => ui.GetNodeID())
            .ToList();

        SetInteractableNodes(startNodes);
    }

    // 스테이지 노드 클릭 시
    private void OnNodeClicked(StageNodeData nodeData)
    {
        switch (nodeData.type)
        {
            case NodeType.Battle:
                GameManager.Instance.StartBattleStage();
                break;

            case NodeType.Shop:
                GameManager.Instance.StartStoreStage();
                break;

            case NodeType.CardUpgrade:
                GameManager.Instance.StartUpgradeStage();
                break;

            case NodeType.Boss:
                GameManager.Instance.StartBossStage();
                break;
        }
        Time.timeScale = 1f;
        gameManager.SelectNode(nodeData);
    }

    // 노드 버튼의 위쪽 중앙 반환
    Vector2 GetTopCenter(RectTransform rt)
    {
        return rt.anchoredPosition + new Vector2(0, rt.rect.height * 0.5f);
    }

    // 노드 버튼의 아래쪽 중앙 반환
    Vector2 GetBottomCenter(RectTransform rt)
    {
        return rt.anchoredPosition - new Vector2(0, rt.rect.height * 0.5f);
    }
}
