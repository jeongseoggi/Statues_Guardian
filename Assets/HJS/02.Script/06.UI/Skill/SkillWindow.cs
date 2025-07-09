using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 스킬 창 클래스(스킬 창 관련하여 처리되는 로직을 모아놓았습니다)
/// </summary>
public class SkillWindow : ActiveUI
{
    #region private
    [SerializeField] private SkillWindowSlot skillPrefab;                     // 스킬 창에 생성 될 prefab
    [SerializeField] private GameObject prefabParent;                         // 생성 될 위치
    [SerializeField] private TextMeshProUGUI skillPointText;                  // 스킬 포인트 Text
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
    public override void ActiveWindow()
    {
        activeWindowMainObject.SetActive(!activeWindowMainObject.activeSelf);
        if (activeWindowMainObject.activeSelf)
        {
            UIManager.Instance.openUIStack.Push(activeWindowMainObject);
        }
        else
        {
            UIManager.Instance.RemoveUI(activeWindowMainObject);
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
    public void OnDestroy()
    {
        GameManager.OnPlayerSkillDataReady -= Init;
        GameManager.Instance.PlayerData.OnSkillPointValueChanged -= SetSkillPoint;
    }


}
