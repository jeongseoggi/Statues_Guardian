using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownAutoClose : MonoBehaviour
{
    public DropDownAnimator dropdownObject;

    public GraphicRaycaster raycaster;              // UI ī�޶� ���� Raycaster
    public EventSystem eventSystem;                 // ���� �̺�Ʈ �ý���

    void Update()
    {
        if (dropdownObject.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            // ���콺 ������ �Ʒ� UI ��ҵ� ��������
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            raycaster.Raycast(pointerData, raycastResults);

            bool clickedDropdown = false;
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject == dropdownObject || result.gameObject.transform.IsChildOf(dropdownObject.transform))
                {
                    clickedDropdown = true;
                    break;
                }
            }

            if (!clickedDropdown)
            {
                dropdownObject.gameObject.SetActive(false);
            }
        }
    }
}
