using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUIManager : SingleTonDestory<DungeonUIManager>
{
    public WaveText waveText;
    public Button nextWaveStartBTN;
    public StageResultText stageResultText;
    public ShopPanelHandler shopPanelHandler;
    public RandomBuffUI randomBuffUI;
    public Button shopButton;
    public DamageTextManager dmgManager;
 

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
        InputManager.OnOpenShop += shopPanelHandler.ShowShopPanel;
    }



    public void ShowResultUI(string resultText, bool isClear)
    {
        GetComponent<Canvas>().sortingOrder = 10;
        stageResultText.ShowResult(resultText, isClear);
    }

    public void DungeonUISetter(bool isActive)
    {
        nextWaveStartBTN.gameObject.SetActive(isActive);
        shopButton.gameObject.SetActive(isActive);
        WaveText.gameObject.SetActive(!isActive);
    }


    public void OnDestroy()
    {
        StageManager.OnStageResult -= ShowResultUI;
        InputManager.OnOpenShop -= shopPanelHandler.ShowShopPanel;
    }
}
