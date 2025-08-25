using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.CloseAllUI();

        switch (scene.name)
        {
            case "BattleScene":
                PlayBGM("BattleBGM", 0.15f);
                break;

            case "MainScene":
                UIManager.Instance.OpenFixedUI(UIType.MainFixed);
                PlayBGM("StartBGM", 0.7f);
                break;

            case "StoreScene":
                UIManager.Instance.OpenFixedUI(UIType.StoreFixed);
                UIManager.Instance.OpenWindow(UIType.GoldWindow);
                break;

            case "UpgradeScene":
                UIManager.Instance.OpenFixedUI(UIType.UpgradeFixed);
                UIManager.Instance.OpenWindow(UIType.GoldWindow);
                break;
        }
    }

    private void PlayBGM(string bgmName, float volume)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.SetBGMVolume(volume);
        SoundManager.Instance.PlayBGM(bgmName);
    }
}
