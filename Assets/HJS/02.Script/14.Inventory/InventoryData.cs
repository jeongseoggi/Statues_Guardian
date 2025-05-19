using UnityEngine;
public class InventoryData
{
    public InventoryData(ItemData itemData, int itemCount)
    {
        ItemData = itemData;
        ItemCount = itemCount;
    }

    private ItemData itemData;
    private int itemCount;

    public int ItemCount { get => itemCount; set => itemCount = value; }
    public ItemData ItemData { get => itemData; set => itemData = value; }
}
