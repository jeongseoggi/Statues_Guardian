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
    public Dictionary<string, InventoryData> itemDict; //�����ۿ� ���� ���� ���� ��ųʸ�
    public event Action<string> itemCountChanged; //������ ���� �̺�Ʈ

    /// <summary>
    /// �÷��̾��� �κ��丮�� ������ ����մϴ�.
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
    /// �÷��̾� �κ��丮���� ������ ��� �� �� ȣ��˴ϴ�.
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
            Debug.LogError("���� �������� ����Ϸ��� �ϰ� ���� ����!");
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
