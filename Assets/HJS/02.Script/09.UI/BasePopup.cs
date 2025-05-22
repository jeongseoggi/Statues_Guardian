using UnityEngine;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    [SerializeField] protected Button okButton;
    [SerializeField] protected Button noButton;

    public virtual void Open() => gameObject.SetActive(true);

    public virtual void Close() => gameObject.SetActive(false);

    public virtual void RemoveButtonListener()
    {
        okButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
    }
}
