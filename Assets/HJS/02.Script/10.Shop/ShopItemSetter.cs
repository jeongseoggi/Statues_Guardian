using UnityEngine;
using UnityEngine.UI;

public class ShopItemSetter : MonoBehaviour
{
    [SerializeField] Item itemPrefab;

    ItemDetail itemDetail;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        itemDetail = transform.parent.GetComponentInChildren<ItemDetail>();

        for (int i = 0; i < DataManager.Instance.ItemDataBase.itemData.Count; i++)
        {
            itemPrefab.itemData = DataManager.Instance.ItemDataBase.itemData[i];
            Instantiate(itemPrefab, gameObject.transform);
        }
    }
}
