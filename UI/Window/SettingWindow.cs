using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SettingWindow : BaseWindow
{
    [Header("BGM, SFX 볼륨 슬라이더")]
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button closeButton;

    [Header("텍스트 표시")]
    public TMP_Text bgmValueText;
    public TMP_Text sfxValueText;

    public override UIType UIType => UIType.SettingWindow;

    void OnEnable()
    {
        // 버튼 활성화(버그픽스)
        closeButton.interactable = true;

        // 팝업 scale 애니메이션
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.28f)
                 .SetEase(Ease.OutBack);

        // 슬라이더 초기값 반영
        bgmSlider.value = SoundManager.Instance.bgmVolume;
        sfxSlider.value = SoundManager.Instance.sfxVolume;

        UpdateText();

        bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        closeButton.onClick.AddListener(Close);

        SoundManager.Instance.PlaySFX("Click");
    }

    void OnDisable()
    {
        // 이벤트 제거(중복 방지)
        bgmSlider.onValueChanged.RemoveListener(OnBGMChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
        closeButton.onClick.RemoveListener(Close);
    }


    void OnBGMChanged(float v)
    {
        SoundManager.Instance.SetBGMVolume(v);
        UpdateText();
    }

    void OnSFXChanged(float v)
    {
        SoundManager.Instance.SetSFXVolume(v);
        UpdateText();
        SoundManager.Instance.PlaySFX("Click");
    }

    void UpdateText()
    {
        if (bgmValueText != null) bgmValueText.text = $"{Mathf.RoundToInt(bgmSlider.value * 100)}";
        if (sfxValueText != null) sfxValueText.text = $"{Mathf.RoundToInt(sfxSlider.value * 100)}";
    }

    void Close()
    {
        // 이미 닫힘 애니메이션 중복 실행 방지
        closeButton.interactable = false;

        // 팝업 닫기 애니메이션
        transform.DOScale(Vector3.zero, 0.18f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                UIManager.Instance.CloseWindow(UIType.SettingWindow);
                SoundManager.Instance.PlaySFX("Click");
            });
    }
}
