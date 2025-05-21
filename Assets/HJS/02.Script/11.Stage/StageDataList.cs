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
    /// ���������� �ش��ϴ� ���� Ÿ�� �迭�� ��ȯ����
    /// </summary>
    /// <param name="index"></param>
    /// <returns>�������� index</returns>
    public int[] GetSpawnMonsterType()
    {
        
        return gameStageDataList[StageIndex].spawnMonsterType;
    }

    /// <summary>
    /// ���� ���������� �ش��ϴ� ���̺��� ���� ���� ��ȯ��
    /// </summary>
    /// <param name="stageIndex"></param>
    /// <param name="wave"></param>
    /// <returns>�������� Index�� ���� ���̺�</returns>
    public int GetMonsterPerWaveCount(int wave)
    {
        return gameStageDataList[StageIndex].monstersPerWave[wave];
    }

    /// <summary>
    /// ���� ���������� �� ���̺� ���� ��ȯ��
    /// </summary>
    /// <param name="stageIndex"></param>
    /// <returns>�������� Index</returns>
    public int GetStageTotalWave()
    {
        return gameStageDataList[StageIndex].totalWave;
    }
}
