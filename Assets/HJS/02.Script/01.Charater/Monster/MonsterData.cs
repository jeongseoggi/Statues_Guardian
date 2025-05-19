using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public Dictionary<MonsterType, MonsterStatData> monsterStatDataDic;
    public List<MonsterStatData> monsterDataList;

    public void Initalize()
    {
        monsterStatDataDic = new Dictionary<MonsterType, MonsterStatData>();
        foreach (var monster in monsterDataList)
        {
            monsterStatDataDic.Add(monster.monsterType, monster);
        }
    }

    public MonsterStatData GetStatData(MonsterType monsterType)
    {
        if (monsterStatDataDic == null)
        {
            Initalize();
        }
        if (monsterStatDataDic.TryGetValue(monsterType, out MonsterStatData monsterstat))
        {
            return monsterstat;
        }
        else
        {
            Debug.LogError("몬스터 타입에 따른 스탯 정보 없음!");
            return null;
        }
            
    }
}
