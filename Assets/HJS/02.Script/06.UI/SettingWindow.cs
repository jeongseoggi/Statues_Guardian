using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingWindow : ActiveUI
{
    [SerializeField] GameObject[] tabMainObjects;
    [SerializeField] TextMeshProUGUI[] tabTexts;
    private int curIndex = -1;


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

    public override void ActiveWindow()
    {
        GameManager.Instance.PlayerInput?.SetActionEnabled(activeWindowMainObject.activeSelf);
        activeWindowMainObject.SetActive(!activeWindowMainObject.activeSelf);
        if (activeWindowMainObject.activeSelf)
        {
            UIManager.Instance.openUIStack.Push(activeWindowMainObject);
            GameManager.Instance.PlayerInput?.SetActionEnabled(false);
            TabButtonAction(0);
        }
        else
        {
            UIManager.Instance.RemoveUI(activeWindowMainObject);
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
}

