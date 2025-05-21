public class UpgradeItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData)
    {
        if (itemData is UpgradeItemData upgradeItemData)
        {
            user.Upgrade(upgradeItemData.upgradeType);
            GameManager.Instance.PlayerInventoryData.UseItem(itemData);
        }
    }
}
