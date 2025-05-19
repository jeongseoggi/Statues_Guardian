using DG.Tweening.Core.Easing;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnManager : PoolableObject<Monster>
{
    public static SpawnManager instance;
    public BoxCollider2D[] spawnZone;
    public int spawnCount;
    public int spawnZonePerMonster;
    public MonsterData monsterData;
    public int aliveCount;
    public Action OnWaveEnd;

    WaveManager waveManager;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        waveManager = GameManager.Instance.WaveManager;
        waveManager.spawnEvent += SetSpawn;
        OnWaveEnd += waveManager.OnEndWave;
    }

    
    public void SetSpawn(int curWave)
    {
        spawnCount = waveManager.stageDataList.GetMonsterPerWaveCount(curWave);

        aliveCount = spawnCount;
        int zoneCount = spawnZone.Length;
        int spawnZonePerMonster = spawnCount / zoneCount;
        int remain = spawnCount % zoneCount;

        // �յ� �й�
        for (int i = 0; i < zoneCount; i++)
        {
            for (int j = 0; j < spawnZonePerMonster; j++)
            {
                SpawnMonsterAtZone(i);
            }
        }

        // ������ �й�
        for (int i = 0; i < remain; i++)
        {
            int randomZoneIndex = UnityEngine.Random.Range(0, zoneCount); // ���Ѱ� ���� ���� ����
            SpawnMonsterAtZone(randomZoneIndex);
        }
    }

    /// <summary>
    /// ���� ���������� �ش��ϴ� ������ Ÿ���� �������� ���� �� ���� ������ ����
    /// </summary>
    /// <param name="zoneIndex"></param>
    void SpawnMonsterAtZone(int zoneIndex)
    {
        Monster mon = base.SpawnPool();
        mon.transform.position = GetRandomPositionInCollider(zoneIndex);
        int monsterTypeCount = waveManager.spawnMonsterTypes.Length;
        int randomIndex = UnityEngine.Random.Range(0, monsterTypeCount);
        int randomType = waveManager.spawnMonsterTypes[randomIndex];

        SpawnMonsterSetting(mon, randomType);
    }

    /// <summary>
    /// ���� �� ���� ����
    /// </summary>
    /// <param name="mon"></param>
    /// <param name="randomType"></param>
    void SpawnMonsterSetting(Monster mon, int randomType)
    {
        mon.MonsterType = (MonsterType)randomType;
        mon.OnDie += HandleMonsterDie;
        mon.gameObject.SetActive(true);
        mon.ResetUI();
        mon.trackingAction?.Invoke();
    }

    /// <summary>
    /// ���� ������ �ȿ����� ���� ���� ����Ʈ ��ȯ
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Vector2 GetRandomPositionInCollider(int index)
    {
        Bounds bounds = spawnZone[index].bounds;

        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        
        return new Vector2(randomX, randomY);
    }

    /// <summary>
    /// ���Ͱ� �׾��� �� ó��
    /// </summary>
    /// <param name="mon"></param>
    public void HandleMonsterDie(Monster mon)
    {
        mon.OnDie -= HandleMonsterDie;
        base.ReturnPool(mon);
        aliveCount--;

        if(aliveCount <= 0)
        {
            OnWaveEnd?.Invoke();
        }
    }


}
