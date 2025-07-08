using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] GameObject[] tabMainObjects;
    [SerializeField] TextMeshProUGUI[] tabTexts;
    [SerializeField] private RectTransform settingWindowPanel;
    [SerializeField] private GameObject settingMainObject;
    private int curIndex = -1;
    private Vector2 offset;


    public void TabButtonAction(int tabIndex)
    {
        if(curIndex == tabIndex)
        {
            return;
        }
        else
        {
            if (curIndex != -1)
            {
                tabTexts[curIndex].color = Color.white;
                tabMainObjects[curIndex].gameObject.SetActive(false);
                tabMainObjects[tabIndex].gameObject.SetActive(true);
                tabTexts[tabIndex].color = Color.yellow;
                curIndex = tabIndex;

                SoundManager.Instance.PlaySFX(DataManager.Instance.GetAudioClip(GameString.UI_CLICK_SOUND));
            }
            else
            {
                tabMainObjects[tabIndex].gameObject.SetActive(true);
                tabTexts[tabIndex].color = Color.yellow;
                curIndex = tabIndex;
            }
        }
    }

    public void ActiveSettingWindow()
    {
        settingMainObject.SetActive(!settingMainObject.activeSelf);
        if (settingMainObject.activeSelf)
        {
            UIManager.Instance.openUIStack.Push(settingMainObject);
            TabButtonAction(0);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region 인터페이스 구현부
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 마우스 위치와 패널 좌상단 사이의 거리 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            settingWindowPanel,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            settingWindowPanel.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            settingWindowPanel.localPosition = localPoint - offset;
        }
    }
    #endregion
}

[System.AttributeUsage(AttributeTargets.Field)]
public class DisplayStatAttribute : Attribute
{
    public string DisplayName;

    public DisplayStatAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}

