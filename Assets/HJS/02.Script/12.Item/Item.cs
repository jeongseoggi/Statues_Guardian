using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public Image itemImage;

    private void OnEnable()
    {
        itemImage.sprite = SpriteManager.Instance.GetSprite(itemData.spriteName);
    }

    public void OnClickItemDetailSet()
    {
        ItemDetail.Instance.SetItemData(itemData);
    }

}
