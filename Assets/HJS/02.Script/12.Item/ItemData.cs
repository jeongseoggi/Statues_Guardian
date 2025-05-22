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

[System.Serializable]
[CreateAssetMenu(menuName = "Item/UpgradeItem")]
public class UpgradeItemData : ItemData
{
    public UpgradeType upgradeType;

    public override void Use(IUseable user, int useCount = 1)
    {
        itemUseStrategy?.Use(user, this, useCount);
    }
}

[System.Serializable]
[CreateAssetMenu(menuName = "Item/HealItem")]
public class HealItemData : ItemData
{
    public HealType healType;
    public float healRatio;

    public override void Use(IUseable user, int useCount= 1)
    {
        itemUseStrategy?.Use(user, this, useCount);
    }
}

[System.Serializable]
[CreateAssetMenu(menuName = "Item/GambleItem")]
public class GambleItemData : ItemData
{
    public override void Use(IUseable user, int useCount)
    {
        itemUseStrategy?.Use(user, this, useCount);
    }
}
