using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public GameObject characterPrefab;
    public int cost;

    [Range(1, 10)]
    public int spawnCount = 1;

    [Range(0.1f, 3f)]
    public float spawnScale = 1f;
}
