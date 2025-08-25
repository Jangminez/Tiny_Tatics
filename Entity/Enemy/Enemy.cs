using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyStat
{
    public int MaxHP { get; private set; }
    public int CurrentHP { get; set; }
    public float MoveSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public float DetectionRange { get; private set; }
    public int AttackDamage { get; private set; }
    public int Gold { get; private set; }
    public void InitializeFromUnitData(UnitData data, int stageLevel)
    {
        MaxHP = (int)data.hp + 10 * stageLevel;
        CurrentHP = MaxHP;
        MoveSpeed = data.moveSpeed;
        AttackRange = data.attackRange;
        DetectionRange = data.attackRange * 2; // TODO: 이후에 detectionRange생기면 교체
        AttackDamage = (int)data.attackDamage + 5 * stageLevel;
        Gold = 15 * stageLevel;
    }
}

public class Enemy : StateMachine, IDamageable
{
    private GameManager gameManager;
    private SoundManager soundManager;
    private UIManager uIManager;

    [SerializeField]
    private string enemyId;

    [SerializeField]
    EnemyStat enemyStat;

    [Header("References")]
    [SerializeField]
    private Transform playerTransform;
    private NavMeshAgent agent;

    public Transform PlayerTransform => playerTransform;
    public NavMeshAgent Agent => agent;
    public AnimationHandler AnimationHandler { get; private set; }
    public UnitData UnitData { get; private set; }

    public IState CurrentState => currentState;

    [HideInInspector]
    public EnemyIdleState idleState;

    [HideInInspector]
    public EnemyChaseState chaseState;

    [HideInInspector]
    public EnemyAttackState attackState;
    private HealthBar_Enemy healthBar;

    [Header("Drop Coin")]
    [SerializeField] GameObject coinPrefab;

    [Header("Enemy Effect")]
    [SerializeField] Transform effectPivot;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        AnimationHandler = new AnimationHandler(GetComponent<Animator>());
    }

    public void Init(int stageLevel)
    {
        gameManager = GameManager.Instance;
        uIManager = UIManager.Instance;
        soundManager = SoundManager.Instance;

        UnitData = gameManager.GameDataManager.GetUnitById(enemyId);

        if (UnitData != null)
        {
            enemyStat.InitializeFromUnitData(UnitData, stageLevel);
        }

        enemyStat.CurrentHP = enemyStat.MaxHP;
        agent.speed = enemyStat.MoveSpeed;

        idleState = new EnemyIdleState(this, enemyStat);
        chaseState = new EnemyChaseState(this, enemyStat);
        attackState = new EnemyAttackState(this, enemyStat);

        ChangeState(idleState);
        var param = new HealthBarParam { TargetTransform = transform, MaxHP = enemyStat.MaxHP };

        healthBar = (HealthBar_Enemy)UIManager.Instance.OpenWorldUI(UIType.HealthBar_Enemy, param);
    }

    public void TakeDamage(int damage)
    {
        enemyStat.CurrentHP -= damage;
        if (healthBar != null)
            healthBar.SetHP(enemyStat.CurrentHP);

        if (soundManager != null && !string.IsNullOrEmpty(UnitData.takeSfxPath))
        {
            soundManager.SetSFXVolume(0.03f);
            soundManager.PlaySFX(System.IO.Path.GetFileName(UnitData.takeSfxPath));
        }

        if (enemyStat.CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (UnitData != null)
        {
            string rarity = UnitData.RarityEnum.ToString();
            string vfxPath = $"VFX/UnitDeath_{rarity}";
            EffectManager.Instance.PlayEffect(vfxPath, transform.position);

            if (!string.IsNullOrEmpty(UnitData.deathSfxPath))
            {
                soundManager.SetSFXVolume(0.03f);
                soundManager.PlaySFX(System.IO.Path.GetFileName(UnitData.deathSfxPath));
            }
        }

        if (healthBar != null)
        {
            UIManager.Instance.CloseWorldUI(healthBar);
            healthBar = null;
        }

        if (coinPrefab != null)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.transform.position = transform.position;
        }

        gameManager.PlayerDataManager.AddGold(enemyStat.Gold);

        Destroy(gameObject);
    }

    public override void Attack()
    {
        if (playerTransform == null) return;

        if (playerTransform.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(enemyStat.AttackDamage);
        }

        if (effectPivot != null)
        {
            EffectManager.Instance.PlayEffect($"VFX/Enemy/{enemyId}_attack", effectPivot.position, effectPivot.rotation);
        }
        // [추가] 공격 효과음
        if (soundManager != null && !string.IsNullOrEmpty(UnitData.attackSfxPath))
        {
            soundManager.SetSFXVolume(0.03f);
            soundManager.PlaySFX(System.IO.Path.GetFileName(UnitData.attackSfxPath));
        }
    }

    public void SetTarget(Collider[] targets)
    {
        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var target in targets)
        {
            if (target == null)
                continue;

            float dist = Vector3.Distance(transform.position, target.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = target.transform;
            }
        }

        playerTransform = nearest;
    }

    private void OnDrawGizmos()
    {
        // 감지 범위 (빨간색 원)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyStat.DetectionRange);

        // 공격 범위 (노란색 원)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyStat.AttackRange);
    }
}
