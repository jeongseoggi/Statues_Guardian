using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BuffItemData", menuName = "Item/BuffItem")]
public class BuffItemData : ItemData
{
    public int buffIds;

    public override void Use(IUseable user, int useCount)
    {
        itemUseStrategy?.Use(user, this, useCount);
    }
}
