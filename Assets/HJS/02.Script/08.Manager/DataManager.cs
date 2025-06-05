using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DataManager : SingleTon<DataManager>
{
    #region private
    private static string ServerURL = "http://localhost:3000/";         // 주소 URL
    [SerializeField] private ItemScriptableObject itemDataBase;         // 아이템을 담은 ScriptableObject
    [SerializeField] private SkillDataList skillDatabase;               // 스킬을 담은 ScriptableObject
    [SerializeField] private List<GameObject> effectList;               // Effect List

    #endregion

    #region 프로퍼티
    public ItemScriptableObject ItemDataBase { get => itemDataBase; }   
    public SkillDataList SkillDataBase { get=> skillDatabase; }
    public List<GameObject> EffectList { get=> effectList; }
    #endregion

    private void Start()
    {
        ItemInit();
        SkillInit();
    }

    /// <summary>
    /// 아이템 DB에서 아이템 데이터를 반환합니다.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public ItemData GetItemData(string itemName)
    {
        return ItemDataBase.itemData.Find((x) => x.itemName.Equals(itemName));
    }

    /// <summary>
    /// 스킬 DB에서 스킬 데이터를 반환합니다.
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public SkillData GetSkillData(string assetName)
    {
        return SkillDataBase.skillDatas.Find((x)=> x.assetName.Equals(assetName));
    }

    /// <summary>
    /// 아이템 타입에 따른 전략 저장
    /// </summary>
    public void ItemInit()
    {
        //타입에 맞는 아이템 전략을 넣어줌
        foreach (var itemData in ItemDataBase.itemData)
        {
            switch(itemData.itemType)
            {
                case ItemType.Heal:
                    itemData.itemUseStrategy = new HealItemStrategy();
                    break;
                case ItemType.Upgrade:
                    itemData.itemUseStrategy = new UpgradeItemStrategy();
                    break;
            }

        }

    }

    /// <summary>
    /// 스킬 타입에 따른 스킬 전략 설정
    /// </summary>
    public void SkillInit()
    {
        foreach(var skillData in SkillDataBase.skillDatas)
        {
            switch(skillData.damageType)
            {
                case DamageType.SpeedUp:
                    skillData.skillStarategy = new SpeedUpStrategy();
                    break;
                case DamageType.AttackUp:
                    skillData.skillStarategy = new AttackUpStrategy();
                    break;
                case DamageType.Dot:
                    skillData.skillStarategy = new DotDamageStrategy();
                    break;
                case DamageType.AoE:
                    skillData.skillStarategy = new AoEStrategy();
                    break;
            }
        }
    }

    /// <summary>
    /// 서버에 데이터를 전송하거나 받을 수 있도록 해주는 코루틴 함수
    /// </summary>
    /// <param name="apiName">api명</param>
    /// <param name="json">서버에 보낼 데이터</param>
    /// <param name="successAction">성공 후의 Action</param>
    /// <returns></returns>
    public static IEnumerator GameConnect(string apiName, WWWForm json, UnityAction<string> successAction)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(ServerURL + apiName, json))
        {
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
#if UNITY_EDITOR
                Debug.Log($"Server Data => {request.downloadHandler.text}");
#endif
                successAction?.Invoke(request.downloadHandler.text);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"[GameConnect] 서버 통신 실패: {request.error}");
#endif
            }
        }
    }
}
