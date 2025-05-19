using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : SingleTonDestory<StageManager>
{
    #region public
    public GameObject stageObject;
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

}
