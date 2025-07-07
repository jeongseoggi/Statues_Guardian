using DG.Tweening.Core.Easing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class NoticePopup : BasePopup
{
    [SerializeField] TextMeshProUGUI mainMsg;
    public void Init(string initText, UnityAction okAction, bool isOneButton = false, UnityAction noAction = null)
    {
        if (isOneButton)
        {
            noButton.gameObject.SetActive(false);
        }
        RemoveButtonListener();
        mainMsg.text = initText;
        okButton.onClick.AddListener(okAction);
        if (noAction != null)
            noButton.onClick.AddListener(noAction);
        Open();
    }


    /// <summary>
    /// 서버 오류 공지 팝업
    /// </summary>
    public void ServerErrorNotice()
    {
        noButton.gameObject.SetActive(false);
        RemoveButtonListener();
        mainMsg.text = GameString.SERVER_ERROR;
        okButton.onClick.AddListener(() =>
        {
#if !UNITY_EDITOR
                    Application.Quit();
#else
            Debug.Log("서버 오류 찾아아함");
            EditorApplication.isPlaying = false;
#endif
        });

        Open();
    }
}
