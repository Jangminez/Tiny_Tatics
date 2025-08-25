using System;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : MonoBehaviour
{
    GameManager gameManager;
    StageDataManager stageDataManager;
    TargetManager targetManager;

    StagePresetData curStagePreset;

    private int curLevel = 1;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        targetManager = gameManager.TargetManager;
        stageDataManager = gameManager.StageDataManager;

        curLevel = 1;
    }

    [ContextMenu("Create Stage By ID")]
    public void TestCreateStage()
    {
        CreateStage(() =>
        {
            Debug.Log("스테이지 생성 완료!");
        });
    }

    public void CreateStage(string stageID, Action onCompleted = null)
    {
        if (curStagePreset == null)
        {
            curStagePreset = stageDataManager.StageLoader.GetByID(stageID);
        }

        CreateObjectInData(curStagePreset);

        onCompleted?.Invoke();
    }

    public void CreateStage(Action onCompleted = null)
    {
        if (curStagePreset == null)
        {
            curStagePreset = stageDataManager.StageLoader.GetRandomStage();
        }

        CreateObjectInData(curStagePreset);

        onCompleted?.Invoke();
    }

    public void CreateBossStage(Action onCompleted = null)
    {
        StagePresetData stageData = stageDataManager.StageLoader.GetBossStage();

        CreateObjectInData(stageData);

        onCompleted?.Invoke();
    }

    private void CreateObjectInData(StagePresetData data)
    {
        targetManager.ClearAllTargets();

        GameObject map = new GameObject("Map");
        Instantiate(stageDataManager.PrefabLoader.GetByID(data.baseMapID), map.transform);

        foreach (UnitPresetData unit in data.units)
        {
            Vector3 position = unit.position.ToVector3();
            Quaternion rotation = unit.rotation.ToQuaternion();

            GameObject unitObj = Instantiate(stageDataManager.PrefabLoader.GetByID(unit.id), position, rotation);

            if (unit.type == ObjectType.Enemy || unit.type == ObjectType.Turret)
            {
                if (unitObj.TryGetComponent(out Enemy enemy))
                {
                    enemy.Init(curLevel);
                    enemy.Agent.Warp(position);
                }

                targetManager.RegisterEnemy(unitObj.transform);
            }

            else if (unit.type == ObjectType.Obstacle)
            {
                targetManager.RegisterBuilding(unitObj.transform);
            }
            
            else if (unit.type == ObjectType.Nexus)
            {
                targetManager.RegisterLastTarget(unitObj.transform);
            }
        }
    }

    public void IncreaseStageLevel()
    {
        curLevel += 1;
    }

    public void ResetStage()
    {
        curLevel = 1;
        curStagePreset = null;
    }

    public void ClearStage()
    {
        curStagePreset = null;
    }
}
