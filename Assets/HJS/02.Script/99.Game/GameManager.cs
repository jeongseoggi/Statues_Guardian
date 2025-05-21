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
    public PlayerStatData PlayerStatData { get; private set; }
    #endregion


    public static event Action<PlayerInventoryData> OnInventoryDataReady;
    public static event Action<int> OnPlayerDataReady;
    public static event Action OnPlayerStatDataReady;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadPlayerData());
    }

    IEnumerator SavePlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.ID);
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

    IEnumerator LoadPlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", 1); //임시 값

        yield return StartCoroutine(DataManager.GameConnect("player/load", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            if (json["id"] != null)
            {
                PlayerData = new PlayerData(json["id"].AsInt, json["level"].AsInt, json["name"], json["stage"].AsInt, json["gold"].AsInt);
                OnPlayerDataReady.Invoke(PlayerData.GetMyGold());
                StartCoroutine(LoadMyInventoryData());
            }
        }));
    }



    IEnumerator LoadMyInventoryData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.ID);

        yield return StartCoroutine(DataManager.GameConnect("inventory/load", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
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
            StartCoroutine(LoadPlayerStatData());
        }));
    }


    IEnumerator LoadPlayerStatData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.ID);

        yield return StartCoroutine(DataManager.GameConnect("playerStat/load", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            if (json["success"].AsBool)
            {
                PlayerStatData = new PlayerStatData();
                PlayerStatData.Hp = json["stats"]["hp"].AsFloat;
                PlayerStatData.Mp = json["stats"]["hp"].AsFloat;
                PlayerStatData.MaxHp = json["stats"]["MaxpHp"].AsFloat;
                PlayerStatData.MaxMp = json["stats"]["MaxMp"].AsFloat;
                PlayerStatData.Atk = json["stats"]["atk"].AsFloat;
                PlayerStatData.Def = json["stats"]["def"].AsFloat;
                PlayerStatData.Speed = json["stats"]["speed"].AsFloat;
                OnPlayerStatDataReady.Invoke();
            }
            else
            {
                Debug.Log(json["message"]);
            }
        }));
    }
}
