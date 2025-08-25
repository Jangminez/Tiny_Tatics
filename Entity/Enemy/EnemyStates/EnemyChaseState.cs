using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IState
{
    private Enemy enemy;
    private EnemyStat enemyStat;
    
    private NavMeshAgent agent;

    public EnemyChaseState(Enemy enemy, EnemyStat enemyStat)
    {
        this.enemy = enemy;
        this.enemyStat = enemyStat;
        this.agent = enemy.Agent;
    }

    public void Enter()
    {
        agent.isStopped = false;
        enemy.AnimationHandler.PlayChase();
    }

    public void Tick()
    {
        // 계속 플레이어 추적
        if (enemy.PlayerTransform == null)
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }
        
        agent.SetDestination(enemy.PlayerTransform.position);

        float sqrDistance = (enemy.PlayerTransform.position - enemy.transform.position).sqrMagnitude;
        float sqrAttack = enemyStat.AttackRange * enemyStat.AttackRange;

        if (sqrDistance <= sqrAttack)
        {
            enemy.ChangeState(enemy.attackState);
        }

        if (sqrDistance > enemyStat.DetectionRange * enemyStat.DetectionRange)
        {
            enemy.ChangeState(enemy.idleState);
        }
    }

    public void Exit()
    {
        agent.isStopped = true;
    }
}
