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
            itemDict[itemData.itemName].ItemCount -= useCount;
            itemCountChanged?.Invoke(itemData.itemName);
            if (itemDict[itemData.itemName].ItemCount <= 0)
            {
                itemDict.Remove(itemData.itemName);
            }
        }
        else
        {
            Debug.LogError("���� �������� ����Ϸ��� �ϰ� ���� ����!");
        }
    }

}
