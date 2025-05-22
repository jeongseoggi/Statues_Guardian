using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DropDownAnimator : MonoBehaviour
{
    public GameObject[] targets;             // 연출할 대상 오브젝트 5개
    public float fadeDuration = 0.2f;        // 각 오브젝트 페이드 시간
    public float interval = 0.15f;            // 다음 오브젝트로 넘어가는 간격
    public InventorySlot curInvenSlot;

    public void ActiveDropDownObject(Vector2 mousePoint, Camera pressEventCamera)
    {
        this.gameObject.SetActive(true);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
               gameObject.transform.parent.GetComponent<RectTransform>(),
               mousePoint,
               pressEventCamera,
               out Vector2 localPoint);

        Vector2 offset = new Vector2(10f, -10f); // 오른쪽 아래
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = localPoint + offset;

    }

    public bool isClickCurData(InventorySlot invenSlot)
    {
        if (curInvenSlot == null)
            return false;

        return curInvenSlot.ItemData.itemName.Equals(invenSlot.ItemData.itemName);
    }

    public void ItemUseButton()
    {
        curInvenSlot.ItemData?.Use(GameManager.Instance.GetPlayer());
        gameObject.SetActive(false);
    }

    public void ItemBundleUseButton()
    {
        InputPopup inputPopup = PopupManager.Instance.inputPopup;

        inputPopup.Init(
            () => 
            {
                int count = inputPopup.inputCount();
                if (count <= 0)
                {
                    inputPopup.Close();
                    return;
                }
                curInvenSlot.ItemData?.Use(GameManager.Instance.GetPlayer(), count);
                inputPopup.Close();
            }, 
            () => { inputPopup.Close(); });
    }
}
