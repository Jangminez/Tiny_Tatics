using System.Collections.Generic;
using UnityEngine;

public class NodeDataHolder : MonoBehaviour
{
    public string nodeID;
    public NodeType type;
    public List<NodeDataHolder> connectedNodeIds;
}
