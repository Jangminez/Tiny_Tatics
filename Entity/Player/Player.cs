using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PlayerStat
{
    //데이터 연결
    public int MaxHP { get; private set; }
    public int CurrentHP { get; set; }
    public int AttackDamage { get; private set; }
    public float AttackInterval { get; private set; }
    public int MoveSpeed { get; private set; }
    public int AttackRange { get; private set; }
    public int DetectRange { get; private set; }
    public string AttackTarget { get; private set; }

    public void InitializeFromUnitData(UnitStat stat, UnitData data)
    {
        MaxHP = (int)stat.MaxHP;
        AttackDamage = (int)stat.AttackDamage;
        AttackInterval = stat.AttackInterval;
        MoveSpeed = (int)stat.MoveSpeed;
        AttackRange = (int)stat.AttackRange;
        DetectRange = (int)stat.AttackRange * 2;
        AttackTarget = data.attackTarget;
    }
}

public class Player : StateMachine, IDamageable
{
    private GameManager gameManager;
    private SoundManager soundManager;
    private UIManager uIManager;

    [SerializeField]
    private string unitId;

    [SerializeField]
    PlayerStat playerStat;
    public PlayerStat Stat => playerStat;

    private NavMeshAgent agent;

    [HideInInspector]
    public Transform target;
    public TargetManager TargetManager { get; private set; }

    public NavMeshAgent Agent => agent;
    public Transform CachedTransform { get; private set; }
    public AnimationHandler AnimationHandler { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    private HealthBar_Player healthBar;
    public UnitData UnitData { get; private set; }

    [Header("Unit Effect")]
    [SerializeField] private Transform effectPivot;

    [Header("Unit FootStep Effect")]
    [SerializeField] private Transform effectPivot_FootStep;
    public Transform EffectPivotFootStep { get; private set; }


    // 실제 소환 체크
    public bool isRealSpawned = false;

    private void Awake()
    {
        CachedTransform = transform;
    }

    public void Init()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        uIManager = UIManager.Instance;

        if (gameManager != null)
        {
            UnitData = gameManager.GameDataManager.GetUnitById(unitId);
            TargetManager = gameManager.TargetManager;
        }

        if (UnitData != null)
        {
            UnitStat unitStat = gameManager.UnitStatManager.GetFinalStat(UnitData);
            playerStat.InitializeFromUnitData(unitStat, UnitData);
        }

        playerStat.CurrentHP = playerStat.MaxHP;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = playerStat.MoveSpeed;
        agent.enabled = true;

        AnimationHandler = new AnimationHandler(GetComponent<Animator>());

        EffectPivotFootStep = effectPivot_FootStep;

        if (isRealSpawned)
        {
            var param = new HealthBarParam
            {
                TargetTransform = transform,
                MaxHP = playerStat.MaxHP,
            };

            healthBar = (HealthBar_Player)uIManager.OpenWorldUI(UIType.HealthBar_Player, param);
        }

        IdleState = new PlayerIdleState(this, playerStat);
        MoveState = new PlayerMoveState(this, playerStat);
        AttackState = new PlayerAttackState(this, playerStat);

        ChangeState(IdleState);
    }

    public void TakeDamage(int damage)
    {
        playerStat.CurrentHP -= damage;

        if (healthBar != null)
            healthBar.SetHP(playerStat.CurrentHP);

        // [추가] 피격 효과음
        if (soundManager != null && !string.IsNullOrEmpty(UnitData.takeSfxPath))
        {
            soundManager.PlaySFX(System.IO.Path.GetFileName(UnitData.takeSfxPath));
        }

        if (playerStat.CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // [추가] 죽음 효과음
        if (soundManager != null && !string.IsNullOrEmpty(UnitData.deathSfxPath))
        {
            soundManager.PlaySFX(System.IO.Path.GetFileName(UnitData.deathSfxPath));
        }

        if (healthBar != null)
        {
            uIManager.CloseWorldUI(healthBar);
            healthBar = null;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        // 감지 범위 (빨간색 원)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerStat.DetectRange);

        // 공격 범위 (노란색 원)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerStat.AttackRange);
    }

    public override void Attack()
    {
        if (target != null && target.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(Stat.AttackDamage);

            if (effectPivot != null)
            {
                EffectManager.Instance.PlayEffect($"VFX/Player/{unitId}_attack", effectPivot.position, effectPivot.rotation);
            }

            if (soundManager != null && !string.IsNullOrEmpty(UnitData.attackSfxPath))
                soundManager.PlaySFX(System.IO.Path.GetFileName(UnitData.attackSfxPath));
        }
    }
}
