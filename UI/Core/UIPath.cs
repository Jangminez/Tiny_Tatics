using System.Collections.Generic;

public static class UIPath
{
    private static readonly Dictionary<UIType, string> pathTable = new()
    {
        // Fixed UI
        { UIType.MainFixed, "UI/FixedUI/MainFixed" },
        { UIType.BattleFixed, "UI/FixedUI/BattleFixed" },
        { UIType.StoreFixed, "UI/FixedUI/StoreFixed" },
        { UIType.UpgradeFixed, "UI/FixedUI/UpgradeFixed" },
        // Window UI
        { UIType.GoldWindow, "UI/WindowUI/GoldWindow" },
        { UIType.StageMapWindow, "UI/WindowUI/StageMapWindow" },
        { UIType.StageClearWindow, "UI/WindowUI/StageClearWindow" },
        { UIType.PauseWindow, "UI/WindowUI/PauseWindow" },
        { UIType.GameClearWindow, "UI/WindowUI/GameClearWindow" },
        { UIType.GameOverWindow, "UI/WindowUI/GameOverWindow" },
        { UIType.SettingWindow, "UI/WindowUI/SettingWindow" },
        // Popup UI
        { UIType.ConfirmPopup, "UI/PopupUI/ConfirmPopup" },
        { UIType.AlertPopup, "UI/PopupUI/AlertPopup" },
        // World UI
        { UIType.HealthBar_Player, "UI/WorldUI/HealthBar_Player" },
        { UIType.HealthBar_Enemy, "UI/WorldUI/HealthBar_Enemy" },
    };

    public static string GetPath(UIType type) => pathTable[type];
}
