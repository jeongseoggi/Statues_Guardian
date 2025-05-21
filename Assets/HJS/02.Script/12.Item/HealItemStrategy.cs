using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class HealItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData)
    {
        if (itemData is HealItemData healItem)
        {
            float healAmount = 0;
            switch (healItem.healType)
            {
                case HealType.HP:
                    healAmount = user.GetMaxHp() * healItem.healRatio;
                    break;
                case HealType.MP:
                    healAmount = user.GetMaxMp() * healItem.healRatio;
                    break;
            }

            user.Heal(healAmount, healItem.healType);
            GameManager.Instance.PlayerInventoryData.UseItem(itemData);
        }
    }
}
