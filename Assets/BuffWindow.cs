using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region private
    [SerializeField] Image buffIcon;
                     string buffDesc;
    #endregion

    #region public
    public string buffName;
    #endregion

    public void Init(Sprite icon, string buffName, string buffDesc)
    {
        buffIcon.sprite = icon;
        this.buffName = buffName;
        this.buffDesc = buffDesc;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManager.Instance.ShowTooltip(buffIcon.sprite, buffName, buffDesc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.HideTooltip();
    }
}
