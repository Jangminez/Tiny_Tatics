using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : BasePopup
{
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI messageText;

    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private Button cancelButton;

    public override UIType UIType => UIType.ConfirmPopup;

    private Action onConfirm;
    private Action onCancel;

    public override void OnOpen(OpenParam param)
    {
        UIEffectHelper.PlayPopupOpen(transform);

        var confirmParam = param as ConfirmPopupParam;

        titleText.text = confirmParam.Title;
        messageText.text = confirmParam.Message;
        onConfirm = confirmParam.OnConfirm;
        onCancel = confirmParam.OnCancel;

        confirmButton.onClick.AddListener(OnClickConfirmButton);
        cancelButton.onClick.AddListener(OnClickCancelButton);
    }

    public void OnClickConfirmButton()
    {
        SoundManager.Instance.SetSFXVolume(0.5f);
        SoundManager.Instance.PlaySFX("Click");

        onConfirm?.Invoke();
    }

    public void OnClickCancelButton()
    {
        SoundManager.Instance.SetSFXVolume(0.5f);
        SoundManager.Instance.PlaySFX("Click");

        UIEffectHelper.PlayPopupClose(
            transform,
            () =>
            {
                onCancel?.Invoke();
                UIManager.Instance.CloseTopPopup();
            }
        );
    }

    public override void OnClose()
    {
        confirmButton.onClick.RemoveListener(OnClickConfirmButton);
        cancelButton.onClick.RemoveListener(OnClickCancelButton);
    }
}
