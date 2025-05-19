using JetBrains.Annotations;
using UnityEngine;

public class HealItemStrategy : IItemUseStrategy
{
    public void Use(IUseable user, ItemData itemData)
    {
        HealType healType = HealType.HP;
        float healAmount = 0;

        switch(itemData.spriteName)
        {
            case "hp":
                healType = HealType.HP;
                healAmount = user.GetMaxHp() * itemData.healRatio;
                break;
            case "mp":
                healType = HealType.HP;
                healAmount = user.GetMaxMp() * itemData.healRatio;
                break;
        }

        user.Heal(healAmount, healType);
        GameManager.Instance.PlayerInventoryData.UseItem(itemData);

    }
}
