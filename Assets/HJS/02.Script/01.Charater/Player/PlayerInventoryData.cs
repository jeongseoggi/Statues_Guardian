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
            itemDict[itemData.itemName].ItemCount -= useCount;
            itemCountChanged?.Invoke(itemData.itemName);
            if (itemDict[itemData.itemName].ItemCount <= 0)
            {
                itemDict.Remove(itemData.itemName);
            }
        }
        else
        {
            Debug.LogError("없는 아이템을 사용하려고 하고 있음 오류!");
        }
    }

}
