using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownAutoClose : MonoBehaviour
{
    public DropDownAnimator dropdownObject;

    public GraphicRaycaster raycaster;              // UI 카메라에 붙은 Raycaster
    public EventSystem eventSystem;                 // 현재 이벤트 시스템

    void Update()
    {
        if (dropdownObject.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            // 마우스 포인터 아래 UI 요소들 가져오기
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
