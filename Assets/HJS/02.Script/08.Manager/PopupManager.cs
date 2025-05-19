using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupManager : SingleTon<PopupManager>
{
    [SerializeField] TextMeshProUGUI mainMsg;
    [SerializeField] Button okButton;
    [SerializeField] Button noButton;

    public void Init(string initText, UnityAction okAction, bool isOneButton = false, UnityAction noAction = null)
    {
        if(isOneButton)
        {
            noButton.gameObject.SetActive(false);
        }
        RemoveButtonListener();
        mainMsg.text = initText;
        okButton.onClick.AddListener(okAction);
        if(noAction != null)
            noButton.onClick.AddListener(noAction);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OkButtonAction(UnityAction okAction)
    {
        okAction?.Invoke();
    }

    public void NoButtonAction(UnityAction noAction)
    {
        noAction?.Invoke();
    }

    public bool CheckActive()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
            return true;
        else
            return false;
    }

    public void RemoveButtonListener()
    {
        okButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
    }

    public void PopupActive(bool isActive)
    {
        transform.GetChild(0).gameObject.SetActive(isActive);
        if(!isActive)
        {
            RemoveButtonListener();
        }
    }
}
