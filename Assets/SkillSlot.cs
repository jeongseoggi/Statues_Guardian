using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    #region private
    [SerializeField] SkillData skillData;
    [SerializeField] Image skillIcon;
    [SerializeField] int slotIndex;
    [SerializeField] GameObject coolTimeObject;
    [SerializeField] TextMeshProUGUI coolTimeText;
    [SerializeField] bool isCoolTime;
    [SerializeField] GameObject dragLayer;
    [SerializeField ]CanvasGroup canvasGroup;
    [SerializeField] GameObject parentObject;
    #endregion

    #region 프로퍼티
    public SkillData SkillData { get => skillData; set => skillData = value; }
    public bool IsCoolTime { get => isCoolTime; set => isCoolTime = value; } 
    #endregion

    private void Start()
    {

    }
    

    public void SetSkillSlot(SkillData skilldata)
    {
        this.SkillData = skilldata;

        if(SkillData != null)
        {
            skillIcon.sprite = SpriteManager.Instance.GetSkillSprite(SkillData.spriteName);
            skillIcon.enabled = true;
        }
        else
        {
            skillIcon.enabled = false;
        }
        SkillManager.Instance.SaveSkillSlotData(slotIndex, SkillData);
    }

    public void StartCoolTime()
    {
        IsCoolTime = true;
        coolTimeObject.SetActive(true);
        StartCoroutine(StartCoolTimeCor());
    }

    IEnumerator StartCoolTimeCor()
    {
        float time = 0;
        coolTimeText.text = skillData.coolTime.ToString();
        while (skillData.coolTime > time)
        {
            time += Time.deltaTime;
            coolTimeText.text = ((int)(skillData.coolTime - time)).ToString();
            yield return null;
        }
        coolTimeObject.SetActive(false);
        IsCoolTime = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        skillIcon.transform.position = eventData.position;

        skillIcon.transform.SetParent(dragLayer.transform);
        skillIcon.transform.SetAsLastSibling();
    }

    public void OnDrop(PointerEventData eventData)
    {
        SkillSlot dragSkillSlot = null;

        if(eventData.pointerDrag.TryGetComponent<SkillSlot>(out dragSkillSlot))
        {
            SkillManager.Instance.SkillSlotSwap(this, dragSkillSlot);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        skillIcon.transform.SetParent(parentObject.transform);
        skillIcon.transform.localPosition = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }
}
