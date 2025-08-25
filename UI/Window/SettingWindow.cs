using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SettingWindow : BaseWindow
{
    [Header("BGM, SFX ���� �����̴�")]
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button closeButton;

    [Header("�ؽ�Ʈ ǥ��")]
    public TMP_Text bgmValueText;
    public TMP_Text sfxValueText;

    public override UIType UIType => UIType.SettingWindow;

    void OnEnable()
    {
        // ��ư Ȱ��ȭ(�����Ƚ�)
        closeButton.interactable = true;

        // �˾� scale �ִϸ��̼�
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.28f)
                 .SetEase(Ease.OutBack);

        // �����̴� �ʱⰪ �ݿ�
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
        // �̺�Ʈ ����(�ߺ� ����)
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
        // �̹� ���� �ִϸ��̼� �ߺ� ���� ����
        closeButton.interactable = false;

        // �˾� �ݱ� �ִϸ��̼�
        transform.DOScale(Vector3.zero, 0.18f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                UIManager.Instance.CloseWindow(UIType.SettingWindow);
                SoundManager.Instance.PlaySFX("Click");
            });
    }
}
