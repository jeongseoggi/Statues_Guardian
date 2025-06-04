using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public RectTransform skillWindowPanel; // 드래그할 전체 스킬 패널
    private Vector2 offset;
    public SkillWindowSlot skillPrefab;
    public GameObject prefabParent;
    public TextMeshProUGUI skillPointText;

    private void Start()
    {
        if (GameManager.Instance?.PlayerSkillData != null)
        {
            Init();
            GameManager.Instance.PlayerData.OnSkillPointValueChanged += SetSkillPoint;
        }
        else
        {
            GameManager.OnPlayerSkillDataReady += Init;
            GameManager.Instance.PlayerData.OnSkillPointValueChanged += SetSkillPoint;
        }
    }

    /// <summary>
    /// 스킬 윈도우 스킬 세팅
    /// </summary>
    public void Init()
    {
        skillPointText.text = "SP : " + GameManager.Instance.PlayerData.SkillPoints.ToString();
        foreach (SkillData skillData in DataManager.Instance.SkillDataBase.skillDatas)
        {
            SkillWindowSlot skillObj = Instantiate(skillPrefab, prefabParent.transform);
            skillObj.SkillSlotSet(
                SpriteManager.Instance.GetSkillSprite(skillData.spriteName),
                skillData.skillName,
                GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName],
                DataManager.Instance.GetSkillData(skillData.assetName).skillMasterLevel
                );
        }
    }

    public void SetSkillPoint(int value)
    {
        skillPointText.text = "SP : " + value.ToString();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        // 마우스 위치와 패널 좌상단 사이의 거리 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            skillWindowPanel,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            skillWindowPanel.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        { 
            skillWindowPanel.localPosition = localPoint - offset;
        }
    }
    public void OnDestroy()
    {
        GameManager.OnPlayerSkillDataReady -= Init;
        GameManager.Instance.PlayerData.OnSkillPointValueChanged -= SetSkillPoint;
    }
}
