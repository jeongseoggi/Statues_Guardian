using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    public QuickSlotManager quickSlotManager;
    public Inventory inventory;
    public DropDownAnimator dropDownAnimator;

    private void OnEnable()
    {
        if(GameManager.Instance?.PlayerInventoryData != null)
            RegisterSlots(GameManager.Instance.PlayerInventoryData);
        else
            GameManager.OnInventoryDataReady += RegisterSlots;
    }

    public void ActiveInventory()
    {
        inventory.ActiveInventory();
    }

    /// <summary>
    /// 각 슬롯에 이벤트 등록
    /// </summary>
    /// <param name="inventoryData"></param>
    private void RegisterSlots(PlayerInventoryData inventoryData)
    {
        foreach (var slot in inventory.inventorySlots)
        {
            slot.Init(slot.ItemData);
        }

        foreach (var qSlot in quickSlotManager.quickSlots)
        {
            qSlot.Init(qSlot.ItemData);
        }
    }

    private void OnDisable()
    {
        GameManager.OnInventoryDataReady -= RegisterSlots;
    }

}
