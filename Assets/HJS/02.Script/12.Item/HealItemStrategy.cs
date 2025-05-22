using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class HealItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData, int useCount = 1)
    {
        if (itemData is HealItemData healItem)
        {
            float healAmount = 0;
            switch (healItem.healType)
            {
                case HealType.HP:
                    healAmount = user.GetMaxHp() * healItem.healRatio * useCount;
                    break;
                case HealType.MP:
                    healAmount = user.GetMaxMp() * healItem.healRatio * useCount;
                    break;
            }

            user.Heal(healAmount, healItem.healType);
            GameManager.Instance.PlayerInventoryData.UseItem(itemData, useCount);
        }
    }
}
