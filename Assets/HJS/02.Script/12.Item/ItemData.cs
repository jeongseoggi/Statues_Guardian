using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public string itemDesc;
    public int price;
    public string spriteName;
    public IItemUseStrategy itemUseStrategy;
    public ItemType itemType;

    [Header("HealItemOnly")]
    public float healRatio;

    public void Use(IUseable user)
    {
        itemUseStrategy?.Use(user, this);
    }
}
