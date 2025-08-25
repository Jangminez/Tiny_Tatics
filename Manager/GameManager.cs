using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public StageManager StageManager { get; private set; }
    public UnitStatManager UnitStatManager { get; private set; }
    public TargetManager TargetManager { get; private set; }

    public StageNodeData PreNode { get; private set; }
    public StageNodeData CurrentNode { get; private set; }

    public GameDataManager GameDataManager { get; private set; }
    public StageDataManager StageDataManager { get; private set; }
    public PlayerDataManager PlayerDataManager { get; private set; }


    private UIManager uIManager;
    private LoadSceneManager loadSceneManager;
    private SoundManager soundManager;
    
    protected override void Awake()
    {
        base.Awake();

        StageManager = GetComponentInChildren<StageManager>();
        UnitStatManager = GetComponentInChildren<UnitStatManager>();

        GameDataManager = GetComponentInChildren<GameDataManager>();
        StageDataManager = GetComponentInChildren<StageDataManager>();
        PlayerDataManager = GetComponentInChildren<PlayerDataManager>();
        
        uIManager = FindObjectOfType<UIManager>();
        soundManager = FindObjectOfType<SoundManager>();

        TargetManager = new TargetManager();

        if (StageManager)
            StageManager.Init(this);
        if (GameDataManager)
            GameDataManager.Init();
        if (PlayerDataManager)
                PlayerDataManager.Init();
        if (UnitStatManager)
            UnitStatManager.Init(this);
    }

    void Start()
    {
        loadSceneManager = LoadSceneManager.Instance;
        SoundManager.Instance.PlayBGM("StartBGM");
    }

    public void StartBattleStage()
    {
        LoadSceneManager.Instance.LoadSceneAsync(
            "BattleScene",
            () =>
            {
                StageManager.CreateStage();
                uIManager.OpenFixedUI(UIType.BattleFixed);
                uIManager.OpenWindow(UIType.GoldWindow);
            }
        );
    }

    public void StartStoreStage()
    {
        loadSceneManager.LoadSceneAsync("StoreScene");
        StageManager.IncreaseStageLevel();
    }

    public void StartUpgradeStage()
    {
        loadSceneManager.LoadSceneAsync("UpgradeScene");
        StageManager.IncreaseStageLevel();
    }

    public void StageClear()
    {
        soundManager.SetSFXVolume(0.3f);
        soundManager.PlaySFX("Victory");

        if (CurrentNode.type == NodeType.Boss)
        {
            uIManager.OpenWindow(UIType.GameClearWindow);

            CurrentNode = null;
            PreNode = null;

            StageManager.ResetStage();
        }
        else
        {
            uIManager.OpenWindow(UIType.StageClearWindow);

            StageManager.ClearStage();
            StageManager.IncreaseStageLevel();
        }
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        soundManager.SetSFXVolume(0.3f);
        soundManager.PlaySFX("Defeat");
        uIManager.OpenWindow(UIType.GameOverWindow);

        CurrentNode = null;
        PreNode = null;

        StageManager.ResetStage();
        Time.timeScale = 0;
    }

    public void SelectNode(StageNodeData nodeData)
    {
        if (CurrentNode != null)
            PreNode = CurrentNode;

        CurrentNode = nodeData;
    }

    public void BackToMain()
    {
        CurrentNode = PreNode;
    }

    public void StartBossStage()
    {
        loadSceneManager.LoadSceneAsync(
            "BattleScene",
            () =>
            {
                StageManager.CreateBossStage();
                uIManager.OpenFixedUI(UIType.BattleFixed);
                uIManager.OpenWindow(UIType.GoldWindow);
            }
        );
    }
}
