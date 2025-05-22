using TMPro;
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
}
