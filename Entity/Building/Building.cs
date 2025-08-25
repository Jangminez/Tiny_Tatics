using UnityEngine;

public class Building : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Animator animator;
    private int hitHash = Animator.StringToHash("Hit");

    [SerializeField]
    private int maxHealth;
    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);

            if (health == 0)
                Die();
        }
    }

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        SoundManager.Instance.SetSFXVolume(0.05f);
        SoundManager.Instance.PlaySFX("Wall_Take");
        Health -= damage;
        animator.SetTrigger(hitHash);
    }

    private void Die()
    {
        if (animator.TryGetComponent(out IDHolder idHolder))
        {
            SoundManager.Instance.SetSFXVolume(0.01f);
            SoundManager.Instance.PlaySFX("Broken");
            Destroy(idHolder.gameObject);

            if (idHolder.type == ObjectType.Nexus)
            {
                GameManager.Instance.StageClear();
            }
        }
    }
}
