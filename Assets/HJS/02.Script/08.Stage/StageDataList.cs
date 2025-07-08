using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataList", menuName = "Stage/StageDataList")]
public class StageDataList : ScriptableObject
{
    int stageIndex;  
    public List<StageData> gameStageDataList;
    public int StageIndex { get => GameManager.Instance.PlayerData.GetCurStage() - 1; }

    public void Initalize()
    {
        gameStageDataList = new List<StageData>();
    }

    /// <summary>
    /// 스테이지에 해당하는 몬스터 타입 배열을 반환해줌
    /// </summary>
    /// <param name="index"></param>
    /// <returns>스테이지 index</returns>
    public int[] GetSpawnMonsterType()
    {
        
        return gameStageDataList[StageIndex].spawnMonsterType;
    }

    /// <summary>
    /// 현재 스테이지에 해당하는 웨이브의 몬스터 수를 반환함
    /// </summary>
    /// <param name="stageIndex"></param>
    /// <param name="wave"></param>
    /// <returns>스테이지 Index와 현재 웨이브</returns>
    public int GetMonsterPerWaveCount(int wave)
    {
        return gameStageDataList[StageIndex].monstersPerWave[wave];
    }

    /// <summary>
    /// 현재 스테이지의 총 웨이브 수를 반환함
    /// </summary>
    /// <param name="stageIndex"></param>
    /// <returns>스테이지 Index</returns>
    public int GetStageTotalWave()
    {
        return gameStageDataList[StageIndex].totalWave;
    }
}
