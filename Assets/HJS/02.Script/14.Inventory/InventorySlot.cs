using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{

    #region private
    [SerializeField] int inventoryIndex;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] ItemData itemData;
    CanvasGroup canvasGroup;
    #endregion

    #region 프로퍼티
    public ItemData ItemData { get => itemData; set => itemData = value; }
    public string ItemCount { get => itemCountText.text; }
    public Image ItemImage { get => itemImage; }
    #endregion
    /// <summary>
    /// 아이템 개수 관련 이벤트 등록 함수
    /// </summary>
    /// <param name="itemData"></param>
    public void Init(ItemData itemData)
    {
        GameManager.Instance.PlayerInventoryData.itemCountChanged += ItemCountChange;
    }

    /// <summary>
    /// 인벤토리 슬롯에 아이템 데이터를 등록합니다.
    /// </summary>
    /// <param name="itemData"></param>
    public void ItemSetting(ItemData itemData)
    {
        if (itemData != null)
        {
            itemImage.enabled = true;
            ItemData = itemData;
            itemImage.sprite = SpriteManager.Instance.GetSprite(itemData.spriteName);
            itemCountText.text = UIManager.Instance.quickSlotManager.GetItemCount(itemData.itemName).ToString();
        }
        else
        {
            itemImage.enabled = false;
            itemCountText.text = string.Empty;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Color color = itemImage.color;
        color.a = 0.3f;
        itemImage.color = color;

        itemImage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Color originColor = itemImage.color;
        originColor.a = 1;
        itemImage.color = originColor;
        canvasGroup.blocksRaycasts = true;

        itemImage.transform.localPosition = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 아이템 카운트 관련 함수
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="count"></param>
    public void ItemCountChange(string itemName)
    {
        if (ItemData != null && ItemData.itemName.Equals(itemName))
        {
            int count = UIManager.Instance.quickSlotManager.GetItemCount(this.ItemData.itemName);
            if (count == 0)
            {
                ClearSlot();
            }
            else
            {
                itemCountText.text = count.ToString();
            }
        }
    }

    public void ClearSlot()
    {
        UIManager.Instance.inventory.RemoveInventoryData(inventoryIndex);
        ItemSetting(ItemData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (this.ItemData == null || string.IsNullOrEmpty(this.ItemData.itemName))
                return;

            DropDownAnimator dropDownAnimator = UIManager.Instance.dropDownAnimator;


            if (!dropDownAnimator.isClickCurData(this))
            {
                dropDownAnimator.curInvenSlot = this;
                dropDownAnimator.gameObject.SetActive(false);
            }

            dropDownAnimator.ActiveDropDownObject(eventData.position, eventData.pressEventCamera);
        }
    }
}

