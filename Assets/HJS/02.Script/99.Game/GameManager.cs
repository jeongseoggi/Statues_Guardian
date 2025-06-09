using JetBrains.Annotations;
using Newtonsoft.Json;
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
    public PlayerSkillData PlayerSkillData { get; private set; }
    #endregion


    public static event Action<PlayerInventoryData> OnInventoryDataReady;
    public static event Action<int> OnPlayerDataReady;
    public static event Action OnPlayerStatDataReady;
    public static event Action OnPlayerSkillDataReady;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadPlayerData());
    }

    /// <summary>
    /// 플레이어 정보 저장
    /// </summary>
    /// <returns></returns>
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
        }));
    }

    /// <summary>
    /// 플레이어 정보 로드
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadPlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", 1); //임시 값

        yield return StartCoroutine(DataManager.GameConnect("player/load", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            if (json["id"] != null)
            {
                PlayerData = new PlayerData(json["id"].AsInt, json["level"].AsInt, json["name"], json["stage"].AsInt, json["gold"].AsInt, json["skillpoints"].AsInt);
                OnPlayerDataReady?.Invoke(PlayerData.GetMyGold());
                StartCoroutine(LoadMyInventoryData());
            }
        }));
    }


    /// <summary>
    /// 플레이어 인벤토리 정보 로드
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMyInventoryData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.ID);

        yield return StartCoroutine(DataManager.GameConnect("inventory/load", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            PlayerInventoryData = new PlayerInventoryData();

            if (json["items"].Count != 0)
            {
                for (int i = 0; i < json["items"].Count; i++)
                {
                    PlayerInventoryData.AddItem(DataManager.Instance.GetItemData(
                        json["items"][i]["item_name"]),
                        json["items"][i]["item_count"]);
                }
            }

            OnInventoryDataReady?.Invoke(PlayerInventoryData);
            StartCoroutine(LoadPlayerStatData());
        }));
    }

    /// <summary>
    /// 플레이어 스탯 정보 로드
    /// </summary>
    /// <returns></returns>
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
                PlayerStatData.MaxHp = json["stats"]["max_hp"].AsFloat;
                PlayerStatData.MaxMp = json["stats"]["max_mp"].AsFloat;
                PlayerStatData.Atk = json["stats"]["atk"].AsFloat;
                PlayerStatData.Def = json["stats"]["def"].AsFloat;
                PlayerStatData.Speed = json["stats"]["speed"].AsFloat;
                PlayerStatData.AttackSpeed = json["stats"]["atkspeed"].AsFloat;
                OnPlayerStatDataReady?.Invoke();
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log(json["message"]);
#endif
            }
            StartCoroutine(LoadPlayerSkillData());
        }));
    }

    /// <summary>
    /// 플레이어 스킬 정보 로드
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadPlayerSkillData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.ID);

        yield return StartCoroutine(DataManager.GameConnect("playerSkill/load", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            if (json["success"].AsBool)
            {
                PlayerSkillData = new PlayerSkillData();
                if (json["skills"].Count == 0)
                {
                    PlayerSkillData.AddNewSkillData();
                    StartCoroutine(SavePlayerSkillData());
                }
                else
                {
                    for(int i =0; i < json["skills"].Count; i++)
                    {
                        PlayerSkillData.playerSkillLevelDic.Add(json["skills"][i]["skill_name"], json["skills"][i]["skill_level"].AsInt);
                    }
                    OnPlayerSkillDataReady?.Invoke();
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log(json["message"]);
#endif
            }
        }));
    }
    
    /// <summary>
    /// 스킬 정보가 없을 경우 기본 값으로 저장
    /// </summary>
    /// <returns></returns>
    IEnumerator SavePlayerSkillData()
    {
        string skillJson = JsonConvert.SerializeObject(PlayerSkillData.playerSkillLevelDic);
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.ID);
        form.AddField("skills", skillJson);

        yield return StartCoroutine(DataManager.GameConnect("playerSkill/save", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            OnPlayerSkillDataReady?.Invoke();
        }));

    }
}
