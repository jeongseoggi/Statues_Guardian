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
    /// 인벤토리에 있는 아이템에 대한 개수를 리턴합니다.
    /// </summary>
    /// <param name="quickSlotItemName"></param>
    /// <returns></returns>
    public int GetItemCount(string quickSlotItemName)
    {
        return GameManager.Instance.PlayerInventoryData.itemDict.TryGetValue(quickSlotItemName, out InventoryData invenData) ? invenData.ItemCount : 0;
    }

    /// <summary>
    /// 퀵 슬롯에 대한 정보를 저장합니다.
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
    /// 킥 슬롯 정보를 로드합니다.
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
    /// 퀵 슬롯 간의 스왑 기능
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
    /// 테스트코드
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
