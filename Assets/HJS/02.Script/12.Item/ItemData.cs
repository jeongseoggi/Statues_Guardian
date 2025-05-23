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

}
