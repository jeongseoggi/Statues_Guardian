using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    #region public
    public QuickSlotManager quickSlotManager;
    public Inventory inventory;
    public DropDownAnimator dropDownAnimator;
    public Stack<GameObject> openUIStack = new Stack<GameObject>();
    public GameObject dragLayer;
    public BuffManager buffManager;
    #endregion

    #region private
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private SkillWindow skillWindow;
                     private Coroutine warningTextCor;
    #endregion

    private void OnEnable()
    {
        RegisterAction();
    }

    /// <summary>
    /// Action 체이닝 함수
    /// </summary>
    public void RegisterAction()
    {
        if (GameManager.Instance?.PlayerInventoryData != null)
        {
            RegisterSlots(GameManager.Instance.PlayerInventoryData);
        }
        else
        {
            GameManager.OnInventoryDataReady += RegisterSlots;
        }

        InputManager.OnInventoryToggle += ActiveInventory;
        InputManager.OnCloseOpenTab += CloseTab;
        InputManager.OnOpenSkillWindow += ActiveSkillWindow;
    }

    /// <summary>
    /// 인벤토리 창 Active 함수
    /// </summary>
    public void ActiveInventory()
    {
        inventory.ActiveInventory();
    }

    /// <summary>
    /// 스킬 창 Active 함수
    /// </summary>
    public void ActiveSkillWindow()
    {
        skillWindow.ActiveSkillWindow();
    }

    /// <summary>
    /// ESC -> 입력 시 창 하나씩 닫아 줄 수 있도록 해주는 함수
    /// </summary>
    public void CloseTab()
    {
        if (openUIStack.Count > 0)
        {
            openUIStack.Pop().gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 각 슬롯에 이벤트 등록
    /// </summary>
    /// <param name="inventoryData"></param>
    private void RegisterSlots(PlayerInventoryData inventoryData)
    {
        foreach (var slot in inventory.inventorySlots)
        {
            slot.Init(slot.ItemData);
        }

        foreach (var qSlot in quickSlotManager.quickSlots)
        {
            qSlot.Init(qSlot.ItemData);
        }
    }

    /// <summary>
    /// 경고 메시지
    /// </summary>
    /// <param name="msg"></param>
    public void SetWarningText(string msg)
    {
        warningText.text = msg;
        warningText.gameObject.SetActive(true);
        if(warningTextCor == null)
        {
            warningTextCor = StartCoroutine(ShowWarningText());
        }
    }

    /// <summary>
    /// 경고 메시지 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowWarningText()
    {
        yield return new WaitForSeconds(1.5f);
        warningText.gameObject.SetActive(false);
        warningTextCor = null;
    }


    private void OnDisable()
    {
        GameManager.OnInventoryDataReady -= RegisterSlots;
        InputManager.OnInventoryToggle -= ActiveInventory;
        InputManager.OnCloseOpenTab -= CloseTab;
    }
}
