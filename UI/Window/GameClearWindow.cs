using UnityEngine;
using UnityEngine.UI;

public class GameClearWindow : BaseWindow
{
    [SerializeField]
    private Button mainMenuButton;
    public override UIType UIType => UIType.GameClearWindow;

    public override void OnOpen()
    {
        UIEffectHelper.PlayPopupOpen(transform);
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
    }

    public override void OnClose()
    {
        mainMenuButton.onClick.RemoveListener(OnClickMainMenu);
    }

    private void OnClickMainMenu()
    {
        UIEffectHelper.PlayPopupClose(
            transform,
            () =>
            {
                UIManager.Instance.CloseWindow(UIType.GameClearWindow);
                LoadSceneManager.Instance.LoadSceneAsync("MainScene");
                Time.timeScale = 1;
            }
        );
    }
}
