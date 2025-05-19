using JetBrains.Annotations;
using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    #region private
    [SerializeField] Player gamePlayer;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] StageManager stageManager;
    [SerializeField] ShopPanelHandler shopPanelHandler;
    [SerializeField] WaveManager waveManager;
    #endregion

    #region 프로퍼티
    public Player GetPlayer() => gamePlayer;
    public SpawnManager SpawnManager { get => spawnManager; }
    public StageManager StageManager { get => stageManager; }
    public ShopPanelHandler ShopPanelHandler { get => shopPanelHandler; }
    public WaveManager WaveManager { get => waveManager; }
    public PlayerData PlayerData { get; private set; }
    public PlayerInventoryData PlayerInventoryData { get; private set; }
    #endregion


    public static event Action<PlayerInventoryData> OnInventoryDataReady;
    public static event Action<int> OnPlayerDataReady;

    protected override void Awake()
    {
        base.Awake();
        PlayerData = new PlayerData(1,1, "테스트", 1, 10000000);

        StartCoroutine(SavePlayerData());
        OnPlayerDataReady.Invoke(PlayerData.GetMyGold());
        StartCoroutine(LoadMyInventoryData());
    }

    IEnumerator SavePlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id",PlayerData.ID);
        form.AddField("level", PlayerData.Level);
        form.AddField("name", PlayerData.NickName);
        form.AddField("stage", PlayerData.Stage);
        form.AddField("gold", PlayerData.Gold);


        yield return StartCoroutine(DataManager.GameConnect("player/save", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            Debug.Log("성공");
        }));
    }

    IEnumerator LoadMyInventoryData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.ID);

        yield return StartCoroutine(DataManager.GameConnect("inventory/load", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            Debug.Log(json);
            PlayerInventoryData = new PlayerInventoryData();

            if (json["items"].Count == 0)
            {
                Debug.Log("인벤토리 없으므로 스킵");
            }
            else
            {
                for (int i = 0; i < json["items"].Count; i++)
                {
                    PlayerInventoryData.AddItem(DataManager.Instance.GetItemData(json["items"][i]["item_name"]), json["items"][i]["item_count"]);
                }
            }

            OnInventoryDataReady?.Invoke(PlayerInventoryData);
        }));
    }
}
