using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapDataSaveTool : MonoBehaviour
{
    [SerializeField] Transform contents;
    [SerializeField] string mapID;

    const string SAVE_PATH = "Assets/Resources/Data/MapData/";

    // StageMapData로 전환
    [ContextMenu("SaveMapData")]
    public void SaveMapData()
    {
        StageMapData mapData = new StageMapData();
        mapData.nodes = new List<StageNodeData>();

        foreach (Transform node in contents)
        {
            if (node.TryGetComponent(out NodeDataHolder dataHolder))
            {
                StageNodeData nodeData = new StageNodeData
                {
                    nodeID = dataHolder.nodeID,
                    type = dataHolder.type,
                    uiPosition = dataHolder.GetComponent<RectTransform>().anchoredPosition,
                    connectedNodeIds = GetConnectedNodes(dataHolder)
                };

                mapData.nodes.Add(nodeData);
            }
        }

        mapData.mapID = mapID;
        DataToJson(mapData);
    }

    // 데이터 JSON 변환
    private void DataToJson(StageMapData mapData)
    {
        string path = $"{SAVE_PATH}{mapID}.json";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        if (File.Exists(path))
        {
            Debug.Log("이미 존재하는 맵데이터ID 입니다");
        }

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(path, json);

        Debug.Log($"맵데이터가 저장되었습니다. 경로 : {path}");
        AssetDatabase.Refresh();
    }

    // 연결된 노드ID 반환
    private List<string> GetConnectedNodes(NodeDataHolder dataHolder)
    {
        List<string> connectedNodes = new List<string>();

        foreach (var connectNode in dataHolder.connectedNodeIds)
        {
            connectedNodes.Add(connectNode.nodeID);
        }

        return connectedNodes;
    }
}
