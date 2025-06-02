using UnityEngine;
using UnityEngine.UI;

public class DungeonUIManager : SingleTonDestory<DungeonUIManager>
{
    public WaveText waveText;
    public Button nextWaveStartBTN;
    public StageResultText stageResultText;

    public WaveText WaveText
    {
        get
        {
            return waveText;
        }
    }
    private void OnEnable()
    {
        StageManager.OnStageClear += ShowClearUI;
        StageManager.OnStageFail += ShowFailUI;
    }


    public void ShowClearUI()
    {
        stageResultText.ShowResult("Stage Clear!");
    }

    public void ShowFailUI()
    {
        stageResultText.ShowResult("Stage Fail!");
    }

    public void OnDestroy()
    {
        StageManager.OnStageClear -= ShowClearUI;
        StageManager.OnStageFail -= ShowFailUI;
    }
}
