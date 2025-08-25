using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : BaseWindow
{
    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Button mainButton;

    private UIManager uiManager;

    public override UIType UIType => UIType.PauseWindow;

    public override void OnOpen()
    {
        UIEffectHelper.PlayPopupOpen(transform);

        uiManager = UIManager.Instance;
        resumeButton.onClick.AddListener(OnClickResume);
        mainButton.onClick.AddListener(OnClickMain);
    }

    public override void OnClose()
    {
        resumeButton.onClick.RemoveListener(OnClickResume);
        mainButton.onClick.RemoveListener(OnClickMain);
    }

    private void OnClickResume()
    {
        UIEffectHelper.PlayPopupClose(
            transform,
            () =>
            {
                uiManager.CloseWindow(UIType.PauseWindow);
                Time.timeScale = 1f;
            }
        );
    }

    private void OnClickMain()
    {
        UIEffectHelper.PlayPopupClose(
            transform,
            () =>
            {
                LoadSceneManager.Instance.LoadSceneAsync("MainScene");
                GameManager.Instance.BackToMain();
                Time.timeScale = 1;
            }
        );
    }
}
