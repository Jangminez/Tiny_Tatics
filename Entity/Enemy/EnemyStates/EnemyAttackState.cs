using UnityEngine;

public class EnemyAttackState : IState
{
    private Enemy enemy;
    private EnemyStat enemyStat;

    private Coroutine attackCoroutine;

    public EnemyAttackState(Enemy enemy, EnemyStat enemyStat)
    {
        this.enemy = enemy;
        this.enemyStat = enemyStat;
    }

    public void Enter()
    {
        enemy.Agent.isStopped = true;

        enemy.AnimationHandler.SetAttackSpeed(1 / enemy.UnitData.attackInterval);
        enemy.AnimationHandler.PlayAttack();
    }

    public void Tick()
    {
        if (enemy.PlayerTransform == null)
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }

        // 공격 범위를 벗어나면 추적 상태로 전환
        float sqrDistance = (enemy.PlayerTransform.position - enemy.transform.position).sqrMagnitude;

        if (sqrDistance > enemyStat.AttackRange * enemyStat.AttackRange)
        {
            enemy.ChangeState(enemy.chaseState);
        }

        LookAtTarget();
    }

    public void Exit()
    {
        if (attackCoroutine != null)
        {
            enemy.StopCoroutine(attackCoroutine);
        }
    }

    private void LookAtTarget()
    {
        if (enemy.PlayerTransform == null) return;

        Vector3 targetPosition = enemy.PlayerTransform.position;
        targetPosition.y = enemy.PlayerTransform.position.y;

        enemy.transform.LookAt(targetPosition);
    }
}
