using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Canvas Roots")]
    [SerializeField]
    private Transform fixedRoot;

    [SerializeField]
    private Transform windowRoot;

    [SerializeField]
    private Transform popupRoot;

    [SerializeField]
    private Transform worldRoot;

    private readonly Dictionary<UIType, BaseWindow> activeWindows = new();
    private readonly Stack<BasePopup> popupStack = new();
    private readonly List<BaseFixed> fixedUIs = new();
    private readonly List<BaseWorld> worldUIs = new();

    private UIPool pool = new();

    // Fixed UI
    public BaseFixed OpenFixedUI(UIType type, OpenParam param = null)
    {
        foreach (var ui in fixedUIs)
            if (ui.UIType == type)
                return ui;

        var newUI = (BaseFixed)pool.GetUI(type, fixedRoot);
        newUI.OnOpen(param);
        newUI.gameObject.SetActive(true);
        fixedUIs.Add(newUI);
        return newUI;
    }

    public void CloseFixed(UIType type)
    {
        for (int i = 0; i < fixedUIs.Count; i++)
        {
            if (fixedUIs[i].UIType == type)
            {
                var ui = fixedUIs[i];
                ui.OnClose();
                pool.ReturnUI(type, ui);
                fixedUIs.RemoveAt(i);
                return;
            }
        }
    }

    // Window UI
    public BaseWindow OpenWindow(UIType type, OpenParam param = null)
    {
        if (activeWindows.ContainsKey(type))
            return activeWindows[type];

        var window = (BaseWindow)pool.GetUI(type, windowRoot);
        window.OnOpen(param);
        window.gameObject.SetActive(true);
        activeWindows[type] = window;
        return window;
    }

    public void CloseWindow(UIType type)
    {
        if (activeWindows.TryGetValue(type, out var window))
        {
            window.OnClose();
            pool.ReturnUI(type, window);
            activeWindows.Remove(type);
        }
    }

    // Popup UI
    public BasePopup OpenPopup(UIType type, OpenParam param = null)
    {
        var popup = (BasePopup)pool.GetUI(type, popupRoot);
        popup.OnOpen(param);
        popup.gameObject.SetActive(true);
        popupStack.Push(popup);
        return popup;
    }

    public void CloseTopPopup()
    {
        if (popupStack.TryPop(out var popup))
        {
            popup.OnClose();
            pool.ReturnUI(popup.UIType, popup);
        }
    }

    // World UI
    public BaseWorld OpenWorldUI(UIType type, OpenParam param = null)
    {
        var ui = (BaseWorld)pool.GetUI(type, worldRoot);
        ui.OnOpen(param);
        worldUIs.Add(ui);
        ui.gameObject.SetActive(true);
        return ui;
    }

    public void CloseWorldUI(BaseWorld ui)
    {
        ui.OnClose();
        pool.ReturnUI(ui.UIType, ui);
        worldUIs.Remove(ui);
    }

    public void CloseAllUI()
    {
        for (int i = fixedUIs.Count - 1; i >= 0; i--)
        {
            var ui = fixedUIs[i];
            ui.OnClose();
            pool.ReturnUI(ui.UIType, ui);
            fixedUIs.RemoveAt(i);
        }

        foreach (var window in activeWindows.Values)
        {
            window.OnClose();
            pool.ReturnUI(window.UIType, window);
        }
        activeWindows.Clear();

        while (popupStack.Count > 0)
        {
            if (popupStack.TryPop(out var popup))
            {
                popup.OnClose();
                pool.ReturnUI(popup.UIType, popup);
            }
        }

        for (int i = worldUIs.Count - 1; i >= 0; i--)
        {
            var ui = worldUIs[i];
            ui.OnClose();
            pool.ReturnUI(ui.UIType, ui);
            worldUIs.RemoveAt(i);
        }
    }

    public T GetWindow<T>()
        where T : BaseWindow
    {
        foreach (var window in activeWindows.Values)
            if (window is T target)
                return target;
        return null;
    }

    public T GetFixed<T>()
        where T : BaseFixed
    {
        foreach (var f in fixedUIs)
        {
            if (f is T target)
                return target;
        }
        return null;
    }

    public BasePopup CurrentPopup => popupStack.Count > 0 ? popupStack.Peek() : null;
}
