using UnityEngine;

public class UILineConnector : MonoBehaviour
{
    public void Connect(Vector2 start, Vector2 end)
    {
        RectTransform rt = GetComponent<RectTransform>();

        Vector2 direction = end - start;
        float length = direction.magnitude;

        rt.sizeDelta = new Vector2(length, 5f);
        rt.anchoredPosition = start + direction * 0.5f;
        rt.rotation = Quaternion.FromToRotation(Vector3.right, direction.normalized);
    }
}
