using System.Collections;
using UnityEngine;

public class PlayerAttackState : IState
{
    private Player player;
    private PlayerStat playerStat;

    private float attackInterval;
    private float lastAttackTime;

    private int enemyLayerMask;

    private Coroutine detectCoroutine;
    private WaitForSeconds waitForSeconds;

    private string unitId;

    Collider[] enemyColliders = new Collider[10];

    public PlayerAttackState(Player player, PlayerStat playerStat)
    {
        this.player = player;
        this.playerStat = playerStat;

        waitForSeconds = new WaitForSeconds(0.2f);

        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public void Enter()
    {
        player.Agent.isStopped = true;

        LookAtTarget();
        attackInterval = playerStat.AttackInterval;

        player.AnimationHandler.SetAttackSpeed(1 / attackInterval);
        player.AnimationHandler.PlayAttack();

        if (player.UnitData != null && !string.IsNullOrEmpty(player.UnitData.attackSfxPath))
            SoundManager.Instance.PlaySFX(
                System.IO.Path.GetFileName(player.UnitData.attackSfxPath)
            );
    }

    public void Tick()
    {
        if (player.target == null)
        {
            player.ChangeState(player.IdleState);
        }

        LookAtTarget();
    }

    public void Exit()
    {
        if (detectCoroutine != null)
        {
            player.StopCoroutine(detectCoroutine);
        }
    }

    private void LookAtTarget()
    {
        if (player.target == null)
            return;

        Vector3 targetPosition = player.target.position;
        targetPosition.y = player.CachedTransform.position.y;

        player.CachedTransform.LookAt(targetPosition);
    }
}
