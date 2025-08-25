using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Player : BaseWorld
{
    [SerializeField]
    private Image fillImage;
    private Transform target;
    private int maxHP;
    private Vector3 offset;
    private float lerpSpeed = 5f;
    private float currentFill = 1f;
    private float targetFill = 1f;
    public override UIType UIType => UIType.HealthBar_Player;

    public override void OnOpen(OpenParam param)
    {
        if (param is HealthBarParam hpParam)
        {
            target = hpParam.TargetTransform;
            maxHP = hpParam.MaxHP;
            SetHP(maxHP);

            float scaleY = hpParam.TargetTransform.localScale.y;
            transform.localScale = Vector3.one * scaleY * 0.3f;
            offset = new Vector3(0, scaleY + 1.5f, 0);
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;

        currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * lerpSpeed);
        fillImage.fillAmount = currentFill;
    }

    public void SetHP(int currentHP)
    {
        targetFill = (float)currentHP / maxHP;
    }
}
