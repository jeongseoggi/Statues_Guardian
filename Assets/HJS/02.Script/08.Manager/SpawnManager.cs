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

        // 균등 분배
        for (int i = 0; i < zoneCount; i++)
        {
            for (int j = 0; j < spawnZonePerMonster; j++)
            {
                SpawnMonsterAtZone(i);
            }
        }

        // 나머지 분배
        for (int i = 0; i < remain; i++)
        {
            int randomZoneIndex = UnityEngine.Random.Range(0, zoneCount); // 상한값 포함 안함 주의
            SpawnMonsterAtZone(randomZoneIndex);
        }
    }

    /// <summary>
    /// 현재 스테이지의 해당하는 몬스터의 타입을 랜덤으로 지정 후 스폰 포지션 지정
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
    /// 스폰 된 몬스터 설정
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
    /// 스폰 포지션 안에서의 랜덤 스폰 포인트 반환
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
    /// 몬스터가 죽었을 때 처리
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
