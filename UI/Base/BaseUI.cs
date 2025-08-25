using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public abstract UIType UIType { get; }

    public virtual void OnOpen() { }

    public virtual void OnOpen(OpenParam param) => OnOpen();

    public virtual void OnClose() { }
}
