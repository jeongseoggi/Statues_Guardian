using UnityEngine;

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
