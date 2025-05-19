using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    #region private
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] GameObject dragLayer;
    private ItemData itemData;
    private GameObject originParentObject;
    private CanvasGroup canvasGroup;
    #endregion

    #region public
    public int slotIndex;
    public string itemName;
    #endregion

    #region 프로퍼티
    public ItemData ItemData { get => itemData; set => itemData = value; }
    #endregion

    private void Start()
    {
        originParentObject = this.gameObject;
    }

    public void Init(ItemData itemData)
    {
        GameManager.Instance.PlayerInventoryData.itemCountChanged += ItemCountChange;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot dragItem = null;

        if (eventData.pointerDrag.TryGetComponent<InventorySlot>(out dragItem))
        {
            ItemData = dragItem.ItemData;
            SetItem(dragItem.ItemData, UIManager.Instance.quickSlotManager.GetItemCount(dragItem.ItemData.itemName));
        }
        else if(eventData.pointerDrag.TryGetComponent<QuickSlot>(out QuickSlot quickSlot))
        {
            UIManager.Instance.quickSlotManager.QuickSlotSwap(this, quickSlot);
        }
    }

    /// <summary>
    /// 퀵 슬롯에 아이템 데이터를 등록합니다.
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="itemCount"></param>
    public void SetItem(ItemData itemData, int itemCount = 0)
    {
        this.ItemData = itemData;
        if (itemData != null)
        {
            if(string.IsNullOrEmpty(itemData.itemName))
            {
                itemImage.enabled = false;
            }
            else
            {
                itemImage.enabled = true;
                itemImage.sprite = SpriteManager.Instance.GetSprite(itemData.spriteName);
            }
            itemCountText.text = itemCount == 0 ? string.Empty : itemCount.ToString();
        }
        else
        {
            itemImage.enabled = false;
            itemCountText.text = string.Empty;
        }
        UIManager.Instance.quickSlotManager.SaveQuickSlotData(slotIndex, itemData);
        
    }

    public void ItemCountChange(string itemName)
    {
        if(ItemData != null && ItemData.itemName.Equals(itemName))
        {
            int count = UIManager.Instance.quickSlotManager.GetItemCount(this.ItemData.itemName);
            if(count == 0)
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
        UIManager.Instance.quickSlotManager.RemoveQuickSlotData(slotIndex); //퀵 슬롯 데이터 제거
        SetItem(ItemData); //UI 업데이트
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemImage.transform.position = eventData.position;

        itemImage.transform.SetParent(dragLayer.transform);
        itemImage.transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.transform.SetParent(originParentObject.transform);
        canvasGroup.blocksRaycasts = true;
        itemImage.transform.localPosition = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup = dragLayer.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }
}
