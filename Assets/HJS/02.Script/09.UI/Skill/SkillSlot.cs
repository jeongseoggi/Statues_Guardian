using System.Collections;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region private
    [SerializeField] SkillData skillData;
    [SerializeField] Image skillIcon;
    [SerializeField] int slotIndex;
    [SerializeField] GameObject coolTimeObject;
    [SerializeField] TextMeshProUGUI coolTimeText;
    [SerializeField] bool isCoolTime;
    [SerializeField] GameObject dragLayer;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject parentObject;
    [SerializeField] bool isNoMana;
    [SerializeField] GameObject noManaPanel;
    #endregion

    #region public
    
    #endregion

    #region 프로퍼티
    public SkillData SkillData { get => skillData; set => skillData = value; }
    public bool IsCoolTime { get => isCoolTime; set => isCoolTime = value; } 
    public bool IsNoMana { get => isNoMana; set => isNoMana = value; }
    #endregion

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
        CheckSkillUse(SkillManager.Instance.player.Mp);
    }

    public void CheckSkillUse(float playerMp)
    {
        if (skillData.mpCost > playerMp)
        {
            noManaPanel.SetActive(true);
            IsNoMana = true;
        }
        else
        {
            noManaPanel.SetActive(false);
            IsNoMana = false;
        }

    }

    #region 인터페이스 구현 함수
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
        else if(eventData.pointerDrag.TryGetComponent<SkillWindowSlot>(out SkillWindowSlot dragSkillWindowSlot))
        {
            SetSkillSlot(dragSkillWindowSlot.SkillData);
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
        canvasGroup = dragLayer.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SkillData == null)
            return;

        float increase = 0;
        int skillLevel = GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName] - 1;

        if (SkillData.damageType == DamageType.AttackUp || SkillData.damageType == DamageType.SpeedUp)
        {
            increase = SkillData.increase + (SkillData.increasePerLevel * skillLevel);
        }
        else
        {
            increase = SkillData.damage + (SkillData.damagePerLevel * skillLevel);
        }

        string skilldesc = string.Format(SkillData.skillDescription, increase);

        ToolTipManager.Instance.ShowTooltip(skillIcon, SkillData.skillName, skilldesc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.HideTooltip();
    }
    #endregion
}
