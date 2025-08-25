using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEffectHelper
{
    public static void PlayPopupOpen(Transform target)
    {
        target.DOKill(true);
        target.localScale = Vector3.zero;
        target.DOScale(Vector3.one, 0.25f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public static void PlayPopupClose(Transform target, System.Action onComplete)
    {
        target.DOKill(true);
        target.DOScale(Vector3.zero, 0.18f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(()=> onComplete?.Invoke());
    }
}
