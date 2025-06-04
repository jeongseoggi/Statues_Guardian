using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : SingleTonDestory<StageManager>
{
    #region public
    public StageObject stageObject;
    public static event Action<string, bool> OnStageResult;
    public SharedHP sharedHp;
    #endregion

    #region private
    WaveManager waveManager;
    ShopPanelHandler shopPanelHandler;
    #endregion

    private void Start()
    {
        shopPanelHandler = GameManager.Instance.ShopPanelHandler;
        waveManager = GameManager.Instance.WaveManager;
        shopPanelHandler.Show();
    }

    public void StartWave()
    {
        shopPanelHandler.Hide();
        waveManager.StartWave();
    }
    public void EndWave()
    {
        shopPanelHandler.Show();
    }

    /// <summary>
    /// 스테이지 성공 처리
    /// </summary>
    public void StageClear()
    {
        OnStageResult?.Invoke("Stage Clear!", true);
    }

    /// <summary>
    /// 스테이지 실패 처리
    /// </summary>
    public void StageFail()
    {
        OnStageResult?.Invoke("Stage Fail...", false);
    }

}
