using UnityEngine;

public class AnimationHandler
{
    private Animator animator;
    int idleHash = Animator.StringToHash("Idle");
    int chaseHash = Animator.StringToHash("Chase");
    int attackHash = Animator.StringToHash("Attack");
    int deathHash = Animator.StringToHash("Death");

    public AnimationHandler(Animator animator)
    {
        this.animator = animator;
    }

    public void PlayIdle()
    {
        animator.Play(idleHash);
    }

    public void PlayChase()
    {
        animator.Play(chaseHash);
    }

    public void PlayAttack()
    {
        animator.Play(attackHash);
    }

    public void PlayDeath()
    {
        animator.Play(deathHash);
    }

    public void SetAttackSpeed(float attackSpeed)
    {
        animator.SetFloat("AttackSpeed", attackSpeed);
    }
}
