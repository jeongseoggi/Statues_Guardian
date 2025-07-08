using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "HealItemData", menuName = "Item/HealItem")]
public class HealItemData : ItemData
{
    public HealType healType;
    public float healRatio;

    public override void Use(IUseable user, int useCount= 1)
    {
        itemUseStrategy?.Use(user, this, useCount);
    }
}
