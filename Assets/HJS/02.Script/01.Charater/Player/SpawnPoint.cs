using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnId;

    public void Awake()
    {
        SpawnPointManager.Register(this);
    }

    private void OnDestroy()
    {
        SpawnPointManager.Unregister(this);
    }
}

public static class SpawnPointManager
{
    private static Dictionary<string, SpawnPoint> spawnPoints = new Dictionary<string, SpawnPoint>();

    public static void Register(SpawnPoint point)
    {
        if (!spawnPoints.ContainsKey(point.spawnId))
        {
            spawnPoints.Add(point.spawnId, point);
        }
        else
        {
            Debug.LogWarning($"Áßº¹µÈ SpawnPoint ID: {point.spawnId}");
        }
    }

    public static void Unregister(SpawnPoint point)
    {
        if (spawnPoints.ContainsKey(point.spawnId))
        {
            spawnPoints.Remove(point.spawnId);
        }
    }


    public static SpawnPoint GetPoint(string id)
    {
        spawnPoints.TryGetValue(id, out var point);
        return point;
    }

    public static void Clear()
    {
        spawnPoints.Clear();
    }

}
