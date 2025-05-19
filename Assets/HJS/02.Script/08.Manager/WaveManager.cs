using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 웨이브의 몬스터 스폰을 담당
/// </summary>
public class WaveManager : MonoBehaviour
{
    #region public
    public static int curWave;
    public static int curStage;
    public int[] spawnMonsterTypes;
    public StageDataList stageDataList;
    public bool waitCountDown;
    public event Action<int> spawnEvent;
    public bool isFinalWave;
    #endregion


    private void Start()
    {
        curWave = 0;
        spawnMonsterTypes = new int[stageDataList.GetSpawnMonsterType().Length];
        spawnMonsterTypes = stageDataList.GetSpawnMonsterType();
        waitCountDown = true;
    }

    /// <summary>
    /// 웨이브 시작에 처리되는 함수
    /// </summary>
    public void StartWave()
    {
        DungeonUIManager.Instance.nextWaveStartBTN.gameObject.SetActive(false);
        if (curWave == stageDataList.GetStageTotalWave() - 1)
        {
            DungeonUIManager.Instance.WaveText.SetText("Final Wave!");
            isFinalWave = true;
        }
        else
        {
            DungeonUIManager.Instance.WaveText.SetText("Wave " +  (curWave + 1));
        }

        StartCoroutine(WaitCountDown());
    }

    /// <summary>
    /// 웨이브가 종료되었을 때 처리되는 함수
    /// </summary>
    public void OnEndWave()
    {
        if (isFinalWave)
        {
            DungeonUIManager.Instance.WaveText.SetText("Stage Clear!", () => { });
        }
        else
        {
            StageManager.Instance.EndWave();
        }
    }

    /// <summary>
    /// 다음 웨이브 카운팅을 기다린 후 몬스터를 스폰합니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitCountDown()
    {
        yield return new WaitUntil(() => !waitCountDown);
        spawnEvent(curWave);
        curWave++;
        waitCountDown = true;
    }
}
