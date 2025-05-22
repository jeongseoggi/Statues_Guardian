using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputPopup : BasePopup
{
    [SerializeField] TMP_InputField inputField;

    public void Init(UnityAction okAction, UnityAction noAction )
    {
        RemoveButtonListener();
        okButton.onClick.AddListener(okAction);
        noButton.onClick.AddListener(noAction);
        Open();
    }

    public int inputCount()
    {
        return int.TryParse(inputField.text, out int inputCount) ? inputCount : 0;
    }

    public override void Close()
    {
        inputField.text = string.Empty;
        base.Close();
    }

}
