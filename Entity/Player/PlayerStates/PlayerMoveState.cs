using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveState : IState
{
    private Player player;
    PlayerStat playerStat;

    private NavMeshAgent agent;

    private int enemyLayerMask;
    private Collider[] enemyColliders = new Collider[10];
    
    private float footstepTimer = 0f;
    private float footstepInterval = 0.4f;

    private const string FOOTSTEP_VFX = "VFX/Player/footstep_effect";

    public PlayerMoveState(Player player, PlayerStat playerStat)
    {
        this.player = player;
        this.playerStat = playerStat;

        this.agent = player.Agent;

        this.enemyLayerMask = LayerMask.GetMask("Enemy");

    }

    public void Enter()
    {
        player.Agent.isStopped = false;
        
        if (player.target != null)
        {
            agent.SetDestination(player.target.position);
        }

        player.AnimationHandler.PlayChase();
    }

    public void Tick()
    {
        if (player.target != null)
        {
            agent.SetDestination(player.target.position);

            float sqrDist = (player.target.position - player.transform.position).sqrMagnitude;
            if (sqrDist <= playerStat.AttackRange * playerStat.AttackRange)
            {
                // 공격 사거리 안에 들어오면 상태 전환
                player.ChangeState(player.AttackState);
            }
            
            footstepTimer += Time.deltaTime;
            
            if (footstepTimer >= footstepInterval)
            {
                footstepTimer = 0f;
                
                if (EffectManager.Instance != null && player.EffectPivotFootStep != null)
                {
                    EffectManager.Instance.PlayEffect(FOOTSTEP_VFX, player.EffectPivotFootStep.position, player.EffectPivotFootStep.rotation);
                }
            }
        }
        else
        {
            player.ChangeState(player.IdleState);
        }
    }

    public void Exit()
    {
        
    }
}
