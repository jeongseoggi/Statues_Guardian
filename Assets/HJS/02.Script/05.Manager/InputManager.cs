using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingleTon<InputManager>
{
    private GameInputActions inputActions;

    public static event Action OnInventoryToggle;
    public static event Action OnCloseOpenTab;
    public static event Action OnOpenSkillWindow;
    public static event Action OnOpenShop;
    public static event Action OnStatInfoWindow;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new GameInputActions();
        inputActions.UI.OpenInventory.performed += ctx => OnInventoryToggle?.Invoke();
        inputActions.UI.CloseTab.performed += ctx => OnCloseOpenTab?.Invoke();
        inputActions.UI.OpenSkillWindow.performed += ctx => OnOpenSkillWindow?.Invoke();
        inputActions.UI.OpenShop.performed += ctx => OnOpenShop?.Invoke();
        inputActions.UI.OpenPlayerInfo.performed += ctx => OnStatInfoWindow?.Invoke();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }
}
