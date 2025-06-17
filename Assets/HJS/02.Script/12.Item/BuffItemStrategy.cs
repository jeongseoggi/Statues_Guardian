using System;
using System.Linq;

public class BuffItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData, int useCount = 1)
    {
        if(itemData is BuffItemData buffItem)
        {
            BuffData buffData = DataManager.Instance.BuffDatabase.buffDataList[buffItem.buffIds];

            // 해당 버프 아이템이 이미 사용중이라면
            if (UIManager.Instance.buffManager.activeBuffWindowList.Find((x) => x.buffName.Equals(buffData.buffName)))
            {
                UIManager.Instance.SetWarningText(GameString.IS_ALREADY_ITEM);
                return;
            }


            if (user is IBuffUsable buffUser)
            {
                DataManager.Instance.BuffDatabase.buffDataList[buffItem.buffIds].BuffEffect(buffUser);
            }
        }

        GameManager.Instance.PlayerInventoryData.UseItem(itemData, useCount);
    }
}
