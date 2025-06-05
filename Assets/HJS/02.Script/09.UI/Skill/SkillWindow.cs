using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 스킬 창 클래스(스킬 창 관련하여 처리되는 로직을 모아놓았습니다)
/// </summary>
public class SkillWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    #region private
    [SerializeField] private RectTransform skillWindowPanel;                  // 드래그할 전체 스킬 패널
    [SerializeField] private SkillWindowSlot skillPrefab;                     // 스킬 창에 생성 될 prefab
    [SerializeField] private GameObject prefabParent;                         // 생성 될 위치
    [SerializeField] private TextMeshProUGUI skillPointText;                  // 스킬 포인트 Text
                     private Vector2 offset;                                  // 드래그에 사용 될 offest


    [Header("SkillWindowMain")]
    [SerializeField] GameObject skillWindowMainObject;      //스킬 메인 창 부모 오브젝트
    #endregion

    private void Start()
    {
        RegisterAction();
    }

    /// <summary>
    /// Action 체이닝 함수
    /// </summary>
    public void RegisterAction()
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
    /// 스킬 창 열고 닫기 함수
    /// </summary>
    public void ActiveSkillWindow()
    {
        skillWindowMainObject.SetActive(!skillWindowMainObject.activeSelf);
        if(skillWindowMainObject.activeSelf)
        {
            UIManager.Instance.openUIStack.Push(skillWindowMainObject);
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
            skillObj.SkillSlotSet(skillData);
        }
    }

    /// <summary>
    /// 스킬 포인트 표기 함수
    /// </summary>
    /// <param name="value"></param>
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
