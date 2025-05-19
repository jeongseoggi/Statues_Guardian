using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public RectTransform inventoryPanel; // �巡���� ��ü �κ��丮 �г�
    private Vector2 offset;
    public InventorySlot[] inventorySlots;
    private Dictionary<int, ItemData> invenSlotData;
    private float saveInterval = 15f;

    [SerializeField] GameObject[] invenObject;

    private void Start()
    {
        invenSlotData = new Dictionary<int, ItemData>();
        Init();
        StartCoroutine(AutoSaveInventory());
    }
    
    /// <summary>
    /// �κ��丮 �ʱ�ȭ �Լ�
    /// </summary>
    private void Init()
    {
        int index = 0;
        foreach(string itemName in GameManager.Instance.PlayerInventoryData.itemDict.Keys)
        {
            if(!invenSlotData.ContainsKey(index))
            {
                invenSlotData.Add(index, DataManager.Instance.GetItemData(itemName));
                inventorySlots[index].ItemSetting(invenSlotData[index]);
            }
            index++;
        }
    }

    public void ActiveInventory()
    {
        foreach(var obj in invenObject)
        {
            obj.SetActive(!obj.activeSelf);
        }

        if (invenObject[0].activeSelf)
            Init();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���콺 ��ġ�� �г� �»�� ������ �Ÿ� ����
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
    /// �κ��丮 �ڵ� ���� �ڷ�ƾ
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
                    Debug.Log($"[Inventory] {pair.Key} ���� �Ϸ�");
                }));
            }
        }
    }
}
