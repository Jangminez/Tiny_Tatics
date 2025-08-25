using System.Collections.Generic;
using UnityEngine;

public class TargetManager
{
    public List<Transform> EnemyTargets { get; private set; } = new List<Transform>();
    public List<Transform> BuildingTargets { get; private set; } = new List<Transform>();
    public Transform LastTarget { get; private set; }
   
    public void RegisterEnemy(Transform enemy)
    {
        if (!EnemyTargets.Contains(enemy))
            EnemyTargets.Add(enemy);
    }

    public void RegisterBuilding(Transform building)
    {
        if (!BuildingTargets.Contains(building))
            BuildingTargets.Add(building);
    }

    public void RegisterLastTarget(Transform lastTarget)
    {
        LastTarget = lastTarget.transform.Find("Target");
    }

    /// <summary>
    /// 타겟이 죽으면 리스트에서 제거
    /// </summary>
    /// <param name="target"></param>
    public void UnRegisterTarget(Transform target)
    {
        if (!EnemyTargets.Remove(target))
            BuildingTargets.Remove(target);
    }

    /// <summary>
    /// 소환된 적들 중에서 가장 가까운 적 반환
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Transform GetNearestEnemy(Vector3 position)
    {
        EnemyTargets.RemoveAll(e => e == null);

        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (Transform t in EnemyTargets)
        {
            if (t == null) continue;

            float dist = Vector3.Distance(position, t.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = t;
            }
        }

        if (EnemyTargets.Count == 0)
            nearest = LastTarget;

        return nearest;
    }

    /// <summary>
    /// 소환된 건물들 중에서 가장 가까운 건물 반환
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Transform GetNearestBuilding(Vector3 position)
    {
        BuildingTargets.RemoveAll(e => e == null);

        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (Transform t in BuildingTargets)
        {
            float dist = Vector3.Distance(position, t.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = t;
            }
        }

        if (BuildingTargets.Count == 0)
            nearest = LastTarget;

        return nearest;
    }

    public void ClearAllTargets()
    {
        EnemyTargets.Clear();
        BuildingTargets.Clear();
    }
}
