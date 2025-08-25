using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    private Player player;
    private PlayerStat playerStat;
    public PlayerIdleState(Player player, PlayerStat playerStat)
    {
        this.player = player;
        this.playerStat = playerStat;
    }
    
    public void Enter()
    {
        player.Agent.isStopped = true;
        if (player.target == null)
        {
            if (playerStat.AttackTarget == "Zombie")
            {
                player.target = player.TargetManager.GetNearestEnemy(player.transform.position);
            }
            else if (playerStat.AttackTarget == "Building")
            {
                player.target = player.TargetManager.GetNearestBuilding(player.transform.position);
            }
        }

        player.AnimationHandler.PlayIdle();    
    }

    public void Tick()
    {
        if (player.target != null)
        {
            player.ChangeState(player.MoveState);
        }
    }

    public void Exit()
    {
        
    }
}
