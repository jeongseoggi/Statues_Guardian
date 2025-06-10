using UnityEngine;
using UnityEngine.UI;

public class DungeonUIManager : SingleTonDestory<DungeonUIManager>
{
    public WaveText waveText;
    public Button nextWaveStartBTN;
    public StageResultText stageResultText;
    public ShopPanelHandler shopPanelHandler;
    public RandomBuffUI randomBuffUI;

    public WaveText WaveText
    {
        get
        {
            return waveText;
        }
    }
    private void OnEnable()
    {
        StageManager.OnStageResult += ShowResultUI;
    }


    public void ShowResultUI(string resultText, bool isClear)
    {
        GetComponent<Canvas>().sortingOrder = 10;
        stageResultText.ShowResult(resultText, isClear);
    }


    public void OnDestroy()
    {
        StageManager.OnStageResult -= ShowResultUI;
    }
}
