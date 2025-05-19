using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ���̺��� ���� ������ ���
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
    /// ���̺� ���ۿ� ó���Ǵ� �Լ�
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
    /// ���̺갡 ����Ǿ��� �� ó���Ǵ� �Լ�
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
    /// ���� ���̺� ī������ ��ٸ� �� ���͸� �����մϴ�.
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
