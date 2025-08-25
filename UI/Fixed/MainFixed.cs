using UnityEngine;
using UnityEngine.UI;

public class MainFixed : BaseFixed
{
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button storeButton;

    [SerializeField]
    private Button upgradeButton;

    [SerializeField]
    private Button settingButton;

    private UIManager uiManager;
    private SoundManager soundManager;

    public override UIType UIType => UIType.MainFixed;

    public override void OnOpen()
    {
        uiManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
        startButton.onClick.AddListener(OnClickStart);
        settingButton.onClick.AddListener(OnClickSetting);
    }

    public void OnClickStart()
    {
        soundManager.SetSFXVolume(0.5f);
        soundManager.PlaySFX("Click");
        uiManager.OpenWindow(UIType.StageMapWindow);
    }

    public void OnClickSetting()
    {
        soundManager.PlaySFX("Click");
        uiManager.OpenWindow(UIType.SettingWindow);
    }


    public override void OnClose()
    {
        startButton.onClick.RemoveListener(OnClickStart);
    }
}
