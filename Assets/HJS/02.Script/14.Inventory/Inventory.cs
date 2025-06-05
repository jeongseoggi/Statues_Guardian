using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    #region private
    [Header("InvenMain")]
    [SerializeField] GameObject invenMainObject;                         // 인벤토리 창 메인 부모 오브젝트

    [SerializeField] private RectTransform inventoryPanel;               // 드래그할 전체 인벤토리 패널
                     private Vector2 offset;                             // 드래그 Offset
                     private Dictionary<int, ItemData> invenSlotData;    // 인벤토리 Slot 딕셔너리
                     private float saveInterval = 15f;                   // 인벤토리 저장 주기(초)
    #endregion

    #region public
    public InventorySlot[] inventorySlots;
    #endregion

    private void Start()
    {
        invenSlotData = new Dictionary<int, ItemData>();
        Init();
        StartCoroutine(AutoSaveInventory());
    }
    
    /// <summary>
    /// 인벤토리 초기화 함수
    /// </summary>
    private void Init()
    {
        int index = 0;
        foreach(string itemName in GameManager.Instance.PlayerInventoryData.itemDict.Keys)
        {
            if (!invenSlotData.ContainsKey(index))
            {
                invenSlotData.Add(index, DataManager.Instance.GetItemData(itemName));
                inventorySlots[index].ItemSetting(invenSlotData[index]);
            }
            index++;
        }
    }

    /// <summary>
    /// 인벤토리 창 온/오프 함수
    /// </summary>
    public void ActiveInventory()
    {
        invenMainObject.SetActive(!invenMainObject.activeSelf);

        if (invenMainObject.activeSelf)
        {
            Init();
            UIManager.Instance.openUIStack.Push(invenMainObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 마우스 위치와 패널 좌상단 사이의 거리 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryPanel,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryPanel.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            inventoryPanel.localPosition = localPoint - offset;
        }
    }

    public void RemoveInventoryData(int inventoryIndex)
    {
        inventorySlots[inventoryIndex].ItemData = null;
        invenSlotData.Remove(inventoryIndex);
    }

    /// <summary>
    /// 인벤토리 자동 저장 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoSaveInventory()
    {
        while(true)
        {
            yield return new WaitForSeconds(saveInterval);

            foreach (var pair in GameManager.Instance.PlayerInventoryData.itemDict)
            {
                WWWForm form = new WWWForm();
                form.AddField("id", GameManager.Instance.PlayerData.ID);
                form.AddField("itemName", pair.Key);
                form.AddField("itemCount", pair.Value.ItemCount);

                yield return StartCoroutine(DataManager.GameConnect("inventory/save", form, (result) =>
                {
#if UNITY_EDITOR
                    Debug.Log($"[Inventory] {pair.Key} 저장 완료");
#endif
                }));
            }
        }
    }
}
