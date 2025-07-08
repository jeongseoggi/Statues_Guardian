using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : SingleTon<ToolTipManager>
{
    #region private
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private Vector2 offset = new Vector2(3f, -3f);
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameTMP;
    [SerializeField] private TextMeshProUGUI itemDescTMP;
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private Canvas canvas;
    #endregion

    void Update()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        if (!tooltipObject.activeSelf)
            return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out mousePos
        );

        Vector2 anchoredPos = mousePos + offset;
        Vector2 canvasSize = (canvas.transform as RectTransform).sizeDelta;
        Vector2 tooltipSize = tooltipRect.sizeDelta;

        // 화면 오른쪽 넘는 경우 왼쪽으로
        if (anchoredPos.x + tooltipSize.x > canvasSize.x / 2)
            anchoredPos.x = canvasSize.x / 2 - tooltipSize.x;

        // 화면 왼쪽 넘는 경우 오른쪽으로
        if (anchoredPos.x < -canvasSize.x / 2)
            anchoredPos.x = -canvasSize.x / 2;

        // 화면 아래 넘는 경우 위로 올림
        if (anchoredPos.y - tooltipSize.y < -canvasSize.y / 2)
            anchoredPos.y = -canvasSize.y / 2 + tooltipSize.y;

        // 화면 위 넘는 경우 아래로 내림
        if (anchoredPos.y > canvasSize.y / 2)
            anchoredPos.y = canvasSize.y / 2;

        tooltipRect.anchoredPosition = anchoredPos;
    }


    public void ShowTooltip(Sprite img, string itemName, string itemDesc)
    {
        itemImage.sprite = img;
        itemNameTMP.text = itemName;
        itemDescTMP.text = itemDesc;


        tooltipObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
