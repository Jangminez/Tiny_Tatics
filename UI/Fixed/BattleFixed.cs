using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleFixed : BaseFixed
{
    [Header("Mana UI")]
    [SerializeField]
    private Image manaFill;

    [SerializeField]
    private TextMeshProUGUI manaText;

    [Header("Card UI")]
    public Image[] cardImages;

    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField] private Image timerFill;

    [SerializeField]
    private Button pauseButton;

    private float maxTime;
    private float remainingTime;
    private bool isTimerRunning;

    public override UIType UIType => UIType.BattleFixed;

    public override void OnOpen(OpenParam param)
    {
        ManaSystem.Instance.OnManaChanged += UpdateManaUI;
        UpdateManaUI();

        pauseButton.onClick.AddListener(OnClickPause);
        StartTimer(120f);
    }

    public override void OnClose()
    {
        pauseButton.onClick.RemoveListener(OnClickPause);
        ManaSystem.Instance.OnManaChanged -= UpdateManaUI;
        StopTimer();
    }

    private void StartTimer(float duration)
    {
        maxTime = duration;
        remainingTime = maxTime;
        isTimerRunning = true;
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }

    private void UpdateManaUI()
    {
        float currentMana = ManaSystem.Instance.GetCurrentMana();
        float maxMana = ManaSystem.Instance.GetMaxMana();

        manaFill.fillAmount = currentMana / maxMana;
        manaText.text = Mathf.FloorToInt(currentMana).ToString();
    }

    private void Update()
    {
        if (!isTimerRunning)
            return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isTimerRunning = false;
            OnTimerEnd();
        }

        timerFill.fillAmount = remainingTime / maxTime;
        timerText.text = FormatTime(remainingTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes}:{seconds:00}";
    }

    private void OnTimerEnd()
    {
        SoundManager.Instance.SetSFXVolume(0.3f);
        SoundManager.Instance.PlaySFX("Defeat");
        // 배틀 패배 처리
        GameManager.Instance.GameOver();
    }

    private void OnClickPause()
    {
        UIManager.Instance.OpenWindow(UIType.PauseWindow);
        Time.timeScale = 0;
    }
}
