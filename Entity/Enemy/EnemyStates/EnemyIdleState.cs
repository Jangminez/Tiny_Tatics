using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : IState
{
    private Enemy enemy;
    private EnemyStat enemyStat;
    private Transform playerTransform;

    private Coroutine detectCoroutine;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);

    private Collider[] playerColliders = new Collider[10];
    private LayerMask playerLayerMask = LayerMask.GetMask("Player");

    public EnemyIdleState(Enemy enemy, EnemyStat enemyStat)
    {
        this.enemy = enemy;
        this.enemyStat = enemyStat;
        this.playerTransform = enemy.PlayerTransform;
    }

    public void Enter()
    {
        enemy.Agent.isStopped = true;

        enemy.AnimationHandler.PlayIdle();
        if (detectCoroutine == null)
        {
            detectCoroutine = enemy.StartCoroutine(DetectPlayerCoroutine());
        }
    }

    private IEnumerator DetectPlayerCoroutine()
    {
        while (true)
        {
            //collider배열을 미리 만들어놓고 거기에 덮어쓰기 방식으로 결과를 채워줘서 새로운 메모리 할당X -> GC Alloc 0
            int count = Physics.OverlapSphereNonAlloc(
                enemy.transform.position,
                enemyStat.DetectionRange,
                playerColliders,
                playerLayerMask
            );

            if (count > 0)
            {
                enemy.SetTarget(playerColliders);

                if (enemyStat.MoveSpeed > 0)
                {
                    enemy.ChangeState(enemy.chaseState);
                }
                else
                {
                    //타워형
                    enemy.ChangeState(enemy.attackState);
                }

                yield break;
            }

            yield return waitForSeconds;
        }
    }

    public void Tick() { }

    public void Exit()
    {
        if (detectCoroutine != null)
        {
            enemy.StopCoroutine(detectCoroutine);
            detectCoroutine = null;
        }
    }
}
