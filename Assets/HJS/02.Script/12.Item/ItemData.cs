using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDesc;
    public int price;
    public string spriteName;
    public IItemUseStrategy itemUseStrategy;
    public ItemType itemType;

    public abstract void Use(IUseable user, int useCount = 1);

    public virtual void Init(string itemName, string itemDesc, int price, string spriteName,
        ItemType itemType)
    {
        this.itemName = itemName;
        this.itemDesc = itemDesc;
        this.price = price;
        this.spriteName = spriteName;
        this.itemType = itemType;
    }
}
