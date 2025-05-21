using DG.Tweening.Core.Easing;
using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryData
{
    public PlayerInventoryData()
    {
        itemDict = new Dictionary<string, InventoryData>();
    }
    public Dictionary<string, InventoryData> itemDict; //아이템에 대한 개수 관련 딕셔너리
    public event Action<string> itemCountChanged; //아이템 변경 이벤트

    /// <summary>
    /// 플레이어의 인벤토리에 아이템 등록합니다.
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="addCount"></param>
    public void AddItem(ItemData itemData, int addCount = 1)
    {
        if(itemDict.ContainsKey(itemData.itemName))
        {
            itemDict[itemData.itemName].ItemCount += addCount;
        }
        else
        {
            itemDict.Add(itemData.itemName, new InventoryData(itemData, addCount));
        }
        itemCountChanged?.Invoke(itemData.itemName);
    }

    /// <summary>
    /// 플레이어 인벤토리에서 아이템 사용 할 때 호출됩니다.
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="useCount"></param>
    public void UseItem(ItemData itemData, int useCount = 1)
    {
        if (itemDict.ContainsKey(itemData.itemName))
        {
            SendToItemChangeData(itemData.itemName, useCount);
        }
        else
        {
            Debug.LogError("없는 아이템을 사용하려고 하고 있음 오류!");
        }
    }

    public void SendToItemChangeData(string itemName, int useCount)
    {


        string statChanges = $"{GameManager.Instance.PlayerStatData.Hp},{GameManager.Instance.PlayerStatData.Mp},{GameManager.Instance.PlayerStatData.Atk},{GameManager.Instance.PlayerStatData.Def}";

        WWWForm form = new WWWForm();
        form.AddField("id", GameManager.Instance.PlayerData.ID);
        form.AddField("item_name", itemName);
        form.AddField("use_count", useCount);
        form.AddField("stat_changes", statChanges);
            
        DataManager.Instance.StartCoroutine(DataManager.GameConnect("inventory/useItem", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);

            Debug.Log(json);

            GameManager.Instance.PlayerStatData.Hp = json["updated_stats"]["hp"].AsFloat;
            GameManager.Instance.PlayerStatData.Mp = json["updated_stats"]["mp"].AsFloat;
            GameManager.Instance.PlayerStatData.Atk = json["updated_stats"]["atk"].AsFloat;
            GameManager.Instance.PlayerStatData.Def = json["updated_stats"]["def"].AsFloat;

            itemDict[itemName].ItemCount = json["updated_item"]["item_count"];
            itemCountChanged?.Invoke(itemName);

            if (itemDict[itemName].ItemCount <= 0)
            {
                itemDict.Remove(itemName);
            }
        }));


    }

}
