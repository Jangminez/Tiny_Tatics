using System.Collections.Generic;
using UnityEngine;

public class StagePresetTool : MonoBehaviour
{
    [Header("Editor Settings")]
    public string stageID;
    public string baseMapID;
    [SerializeField] Transform enemySpawnRoot;
    [SerializeField] Transform turretSpawnRoot;
    [SerializeField] Transform obstacleSpawnRoot;
    [SerializeField] Transform nexusSpawnRoot;

    public List<UnitPresetData> GetUnitPresetData()
    {
        List<UnitPresetData> unitPresetDatas = new List<UnitPresetData>();

        CreateUnitPresetData(unitPresetDatas, enemySpawnRoot);
        CreateUnitPresetData(unitPresetDatas, turretSpawnRoot);
        CreateUnitPresetData(unitPresetDatas, obstacleSpawnRoot);
        CreateUnitPresetData(unitPresetDatas, nexusSpawnRoot);

        return unitPresetDatas;
    }

    private void CreateUnitPresetData(List<UnitPresetData> unitPresetDatas, Transform root)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out IDHolder idHolder))
            {
                UnitPresetData unitPresetData = new UnitPresetData(
                    idHolder.type,
                    idHolder.id,
                    new SerializableVector3(child.position),
                    new SerializableQuaternion(child.rotation)
                );

                unitPresetDatas.Add(unitPresetData);
            }
        }
    }
}
