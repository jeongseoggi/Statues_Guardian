using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Item/GambleItem")]
public class GambleItemData : ItemData
{
    public override void Use(IUseable user, int useCount)
    {
        itemUseStrategy?.Use(user, this, useCount);
    }
}
