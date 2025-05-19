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
        if (Int32.TryParse(amountInputField.text, out count))
        {
            if (count < 0)
            {
                PopupManager.Instance.Init("0 �̻��� ���� �Է����ּ���!",
              () =>
              {
                  PopupManager.Instance.PopupActive(false);
              }, true);

                return;
            }
        }
        else
        {
            PopupManager.Instance.Init("��Ȯ�� ���� �Է����ּ���!",
              () =>
              {
                  PopupManager.Instance.PopupActive(false);
              }, true);

            return;
        }

        ItemDetail.Instance.SetItemCount(count);
        this.gameObject.SetActive(false);
    }
}
