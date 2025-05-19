using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Item/ItemScriptableObject")]
public class ItemScriptableObject : ScriptableObject
{
    public List<ItemData> itemData;

    public void Initalize()
    {
        itemData = new List<ItemData>();
    }
}
