using SimpleJSON;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillWindowSlot : MonoBehaviour, IPointerEnterHandler, IDragHandler, 
    IEndDragHandler, IBeginDragHandler, IPointerExitHandler
{
    #region private
    [SerializeField] private SkillData          skilldata;          //스킬 데이터
    [SerializeField] private Image              skillIcon;          //스킬 아이콘
    [SerializeField] private TextMeshProUGUI    skillName;          //스킬 이름
    [SerializeField] private TextMeshProUGUI    skillLevelTMP;      //스킬 레벨 TMP
    [SerializeField] private Button             levelUpButton;      //레벨 업 버튼
    [SerializeField] private int                skillMasterLv;      //스킬 마스터 레벨
    [SerializeField] private int                curSkillLevel;      //현재 스킬 레벨
    [SerializeField] private GameObject         originParentObject; //스킬 아이콘 부모 오브젝트
                     private CanvasGroup        canvasGroup;        //드래그 시 사용 될 CanvasGroup
                     private Vector2            skillIconOriginPos;
    #endregion

    #region 프로퍼티
    public SkillData SkillData { get => skilldata; set => skilldata = value; }
    public int CurSkillLevel { get => curSkillLevel; set => curSkillLevel = value; }
    #endregion
    private void Start()
    {
        canvasGroup = UIManager.Instance.dragLayer.GetComponent<CanvasGroup>();
        skillIconOriginPos = skillIcon.gameObject.transform.localPosition;
    }

    public void SkillSlotSet(SkillData skillData)
    {
        SkillData = skillData;
        this.skillIcon.sprite = SpriteManager.Instance.GetSkillSprite(skillData.spriteName);
        this.skillName.text = skillData.skillName;
        this.curSkillLevel = GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName];
        skillLevelTMP.text = curSkillLevel.ToString();
        this.skillMasterLv = DataManager.Instance.GetSkillData(skillData.assetName).skillMasterLevel;
    }

    public void UpGradeSkill()
    {
        if (GameManager.Instance.PlayerData.SkillPoints <= 0)
            return;

        if (skillMasterLv <= curSkillLevel)
            return;

        levelUpButton.interactable = false;
        StartCoroutine(SkillLevelUp());
    }

    IEnumerator SkillLevelUp()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", GameManager.Instance.PlayerData.ID);
        form.AddField("skillName", skillName.text);
        yield return StartCoroutine(DataManager.GameConnect("playerSkill/levelUp", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            if (json["success"].AsBool)
            {
                levelUpButton.interactable = true;
                curSkillLevel = json["skill"]["skill_level"].AsInt;
                this.skillLevelTMP.text = curSkillLevel.ToString();
                GameManager.Instance.PlayerData.SkillPoints = json["remainingSkillpoints"].AsInt;
                GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillName.text] = curSkillLevel;
            }

        }));
    }

    #region 인터페이스 구현 함수
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Color color = skillIcon.color;
        color.a = 0.3f;
        skillIcon.color = color;

        skillIcon.transform.position = eventData.position;

        skillIcon.transform.SetParent(UIManager.Instance.dragLayer.transform);
        skillIcon.transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Color originColor = skillIcon.color;
        originColor.a = 1;
        skillIcon.color = originColor;
        canvasGroup.blocksRaycasts = true;

        skillIcon.transform.SetParent(originParentObject.transform);

        skillIcon.transform.localPosition = skillIconOriginPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        float increase = 0;
        int skillLevel = GameManager.Instance.PlayerSkillData.playerSkillLevelDic[SkillData.skillName] - 1;

        if (SkillData.damageType == DamageType.AttackUp || SkillData.damageType == DamageType.SpeedUp)
        {
            increase = SkillData.increase + (SkillData.increasePerLevel * skillLevel);
        }
        else
        {
            increase = SkillData.damage+ (SkillData.damagePerLevel * skillLevel);
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
