using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ActiveUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] protected RectTransform activeUIPanel;
    [SerializeField] protected GameObject activeWindowMainObject;
    protected Vector2 offset;

    public abstract void ActiveWindow();

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        // 마우스 위치와 패널 좌상단 사이의 거리 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            activeUIPanel,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            activeUIPanel.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            activeUIPanel.localPosition = localPoint - offset;
        }
    }
}
