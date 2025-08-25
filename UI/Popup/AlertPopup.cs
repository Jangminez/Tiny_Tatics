using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertPopup : BasePopup
{
    [SerializeField]
    private Button alertPopup;

    [SerializeField]
    private TextMeshProUGUI messageText;
    private Coroutine closeRoutine;
    public override UIType UIType => UIType.AlertPopup;

    public override void OnOpen(OpenParam param)
    {
        UIEffectHelper.PlayPopupOpen(transform);

        var alertParam = param as AlertPopupParam;
        messageText.text = alertParam.Message;

        alertPopup.onClick.AddListener(OnClickAlertPopup);
        closeRoutine = StartCoroutine(DelayedClose());
    }

    private void OnClickAlertPopup()
    {
        if (closeRoutine != null)
        {
            StopCoroutine(closeRoutine);
            closeRoutine = null;
        }

        UIEffectHelper.PlayPopupClose(
            transform,
            () =>
            {
                UIManager.Instance.CloseTopPopup();
            }
        );
    }

    private IEnumerator DelayedClose()
    {
        yield return new WaitForSeconds(2f);
        UIEffectHelper.PlayPopupClose(
            transform,
            () =>
            {
                UIManager.Instance.CloseTopPopup();
            }
        );
    }

    public override void OnClose()
    {
        alertPopup.onClick.RemoveListener(OnClickAlertPopup);

        if (closeRoutine != null)
        {
            StopCoroutine(closeRoutine);
            closeRoutine = null;
        }
    }
}
