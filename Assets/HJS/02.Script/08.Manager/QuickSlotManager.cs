using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public QuickSlot[] quickSlots;
    public Dictionary<int, string> quickSlotSaveDic = new Dictionary<int, string>();

    private void Start()
    {
        LoadQuickSlotData();
    }

    /// <summary>
    /// �κ��丮�� �ִ� �����ۿ� ���� ������ �����մϴ�.
    /// </summary>
    /// <param name="quickSlotItemName"></param>
    /// <returns></returns>
    public int GetItemCount(string quickSlotItemName)
    {
        return GameManager.Instance.PlayerInventoryData.itemDict.TryGetValue(quickSlotItemName, out InventoryData invenData) ? invenData.ItemCount : 0;
    }

    /// <summary>
    /// �� ���Կ� ���� ������ �����մϴ�.
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="itemData"></param>
    public void SaveQuickSlotData(int slotIndex, ItemData itemData)
    {
        if (itemData == null)
        {
            if (quickSlotSaveDic.ContainsKey(slotIndex))
            {
                quickSlotSaveDic.Remove(slotIndex);
            }
        }
        else
        {
            if (quickSlotSaveDic.ContainsKey(slotIndex))
            {
                quickSlotSaveDic[slotIndex] = itemData.itemName;
            }
            else
            {
                if (!string.IsNullOrEmpty(itemData.itemName))
                {
                    quickSlotSaveDic.Add(slotIndex, itemData.itemName);
                }

            }
        }

        string json = JsonConvert.SerializeObject(quickSlotSaveDic);
        PlayerPrefs.SetString("QuickSlotData", json);
    }

    /// <summary>
    /// ű ���� ������ �ε��մϴ�.
    /// </summary>
    public void LoadQuickSlotData()
    {
        if (!PlayerPrefs.HasKey("QuickSlotData"))
        {
            return;
        }
        else
        {
            string json = PlayerPrefs.GetString("QuickSlotData");
            Debug.Log(json);
            quickSlotSaveDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);

            foreach (var pair in quickSlotSaveDic.ToList())
            {
                int index = pair.Key;
                string itemName = pair.Value;


                int count = GetItemCount(itemName);
                quickSlots[index].SetItem(DataManager.Instance.GetItemData(itemName), count);
            }
        }
    }

    /// <summary>
    /// �� ���� ���� ���� ���
    /// </summary>
    /// <param name="targetQuickSlot"></param>
    /// <param name="swapQuickSlot"></param>
    public void QuickSlotSwap(QuickSlot targetQuickSlot, QuickSlot swapQuickSlot)
    {
        ItemData tempQuickSlot = targetQuickSlot.ItemData;
        targetQuickSlot.SetItem(swapQuickSlot.ItemData, GetItemCount(swapQuickSlot.ItemData.itemName));
        if (tempQuickSlot != null)
        {
            swapQuickSlot.SetItem(tempQuickSlot, GetItemCount(tempQuickSlot.itemName));
        }
        else
        {
            swapQuickSlot.SetItem(tempQuickSlot);
        }
    }

    public void RemoveQuickSlotData(int slotIndex)
    {
        quickSlots[slotIndex].ItemData = null;
        SaveQuickSlotData(slotIndex, quickSlots[slotIndex].ItemData);
    }

    /// <summary>
    /// �׽�Ʈ�ڵ�
    /// </summary>
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            if (PlayerPrefs.HasKey("QuickSlotData"))
            {
                PlayerPrefs.DeleteKey("QuickSlotData");
            }
        }
    }

}
