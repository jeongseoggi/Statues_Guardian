using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
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
        shopPanelHandler = DungeonUIManager.Instance.shopPanelHandler;
        waveManager = GameManager.Instance.WaveManager;
        shopPanelHandler.Show();
    }

    public void StartWave()
    {
        shopPanelHandler.Hide();
        waveManager.StartWave();
        GameManager.GameState = GameState.Play;
    }
    public void EndWave()
    {
        shopPanelHandler.Show();
        GameManager.GameState = GameState.Wait;
    }

    /// <summary>
    /// 스테이지 성공 처리
    /// </summary>
    public void StageClear()
    {
        OnStageResult?.Invoke(GameString.STAGE_CLEAR, true);
        StartCoroutine(SendToServerStageReward());
    }

    /// <summary>
    /// 스테이지 실패 처리
    /// </summary>
    public void StageFail()
    {
        OnStageResult?.Invoke(GameString.STAGE_FAIL, false);
    }

    public IEnumerator SendToServerStageReward()
    {
        RewardData reward = DataManager.Instance.GetRewardData();
        WWWForm form = new WWWForm();

        Dictionary<string, int> rewardDic = new Dictionary<string, int>();

        for(int i = 0; i < reward.itemIDs.Count; i++)
        {
            rewardDic.Add(DataManager.Instance.GetItemData(reward.itemIDs[i]).itemName,
                reward.amounts[i]);
        }

        string rewardJson = JsonConvert.SerializeObject(rewardDic);
        

        form.AddField("id", GameManager.Instance.PlayerData.ID);
        form.AddField("reward", rewardJson);
        form.AddField("gold", reward.gold);

        yield return StartCoroutine(DataManager.GameConnect("playerStageReward/stageReward", form, data=>
        {
            JSONNode jsonData = JSONNode.Parse(data);

            if (jsonData["success"])
            {
                // 인벤토리 및 골드 업데이트
                foreach(var kvp in rewardDic)
                {
                    GameManager.Instance.PlayerInventoryData.AddItem(DataManager.Instance.GetItemData(kvp.Key), kvp.Value);
                }
                GameManager.Instance.PlayerData.Gold += reward.gold;
            }
        }));
    }

}





