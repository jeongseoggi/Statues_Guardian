using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class AmountInputPopup : MonoBehaviour
{
    [SerializeField] TMP_InputField amountInputField;

    public void OnClickApply()
    {
        int count = 0;
        NoticePopup noticePopup = PopupManager.Instance.noticePopup;
        if (Int32.TryParse(amountInputField.text, out count))
        {
            if (count < 0)
            {
                noticePopup.Init("0 이상의 값을 입력해주세요!",
                () =>
                {
                    noticePopup.Close();
                }, true);

                return;
            }
        }
        else
        {
            noticePopup.Init("정확한 값을 입력해주세요!",
            () =>
            {
                noticePopup.Close();
            }, true);

            return;
        }

        ItemDetail.Instance.SetItemCount(count);
        this.gameObject.SetActive(false);
    }
}
