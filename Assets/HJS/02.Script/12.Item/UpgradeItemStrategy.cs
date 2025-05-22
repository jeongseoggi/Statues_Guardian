public class UpgradeItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData, int useCount = 1)
    {
        if (itemData is UpgradeItemData upgradeItemData)
        {
            user.Upgrade(upgradeItemData.upgradeType, useCount);
            GameManager.Instance.PlayerInventoryData.UseItem(itemData, useCount);
        }
    }
}
