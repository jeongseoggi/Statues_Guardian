public class BuffItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData, int useCount = 1)
    {
        if(itemData is BuffItemData buffItem)
        {
            if(user is IBuffUsable buffUser)
            {
                DataManager.Instance.BuffDatabase.buffDataList[buffItem.buffIds].BuffEffect(buffUser);
            }
        }

        GameManager.Instance.PlayerInventoryData.UseItem(itemData, useCount);
    }
}
