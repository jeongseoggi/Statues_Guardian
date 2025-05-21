using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDesc;
    public int price;
    public string spriteName;
    public IItemUseStrategy itemUseStrategy;
    public ItemType itemType;

    public abstract void Use(IUseable user);

}

[System.Serializable]
[CreateAssetMenu(menuName = "Item/UpgradeItem")]
public class UpgradeItemData : ItemData
{
    public UpgradeType upgradeType;

    public override void Use(IUseable user)
    {
        itemUseStrategy?.Use(user, this);
    }
}

[System.Serializable]
[CreateAssetMenu(menuName = "Item/HealItem")]
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
[CreateAssetMenu(menuName = "Item/GambleItem")]
public class GambleItemData : ItemData
{
    public override void Use(IUseable user)
    {
        itemUseStrategy?.Use(user, this);
    }
}
