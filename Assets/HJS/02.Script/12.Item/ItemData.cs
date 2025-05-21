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

    public virtual void Use(IUseable user)
    {

    }

}

[System.Serializable]
public class UpgradeItemData : ItemData
{
    public UpgradeType upgradeType;

    public override void Use(IUseable user)
    {
        itemUseStrategy?.Use(user, this);
    }
}

[System.Serializable]
public class HealItemData : ItemData
{
    public HealType healType;
    public float healRatio;

    public override void Use(IUseable user)
    {
        itemUseStrategy?.Use(user, this);
    }
}

[System.Serializable]
public class GambleItemData : ItemData
{
    public override void Use(IUseable user)
    {
        itemUseStrategy?.Use(user, this);
    }
}
