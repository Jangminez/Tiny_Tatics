using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : BaseWindow
{
    [SerializeField]
    private Button mainMenuButton;
    public override UIType UIType => UIType.GameOverWindow;

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
                UIManager.Instance.CloseWindow(UIType.GameOverWindow);
                LoadSceneManager.Instance.LoadSceneAsync("MainScene");
                Time.timeScale = 1;
            }
        );
    }
}
