using System;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 플레이어 인풋 관련 처리 클래스
/// </summary>
public class PlayerActionInput : MonoBehaviour
{
    #region private
    [SerializeField] private PlayerSkillController  playerskillController;
    [SerializeField] private Player                 player;
    [SerializeField] private PlayerItemHandler      playerItemHandler;
    [SerializeField] private PlayerController       playerController;
    #endregion

    #region public
    public static event Action<int, ISkillUable> OnSkillUse;
    public static event Action<int, IUseable> OnItemlUse;
    public static event Action OnMoveAction;
    public PlayerInputActions playerInputActions;
    public static Vector2 CurrentInput;
    #endregion

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        RegisterBinding();
    }

    private void RegisterBinding()
    {
        playerInputActions.Player.Skill1.performed += OnSkill1;
        playerInputActions.Player.Skill2.performed += OnSkill2;
        playerInputActions.Player.Skill3.performed += OnSkill3;
        playerInputActions.Player.Skill4.performed += OnSkill4;

        playerInputActions.Player.ItemUse1.performed += OnItem1;
        playerInputActions.Player.ItemUse2.performed += OnItem2;
        playerInputActions.Player.ItemUse3.performed += OnItem3;
        playerInputActions.Player.ItemUse4.performed += OnItem4;


        playerInputActions.Player.Move.performed += OnMovePerformed;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;

        playerInputActions.Player.Attack.performed += OnAttack;
    }

    private void UnregisterBinding()
    {
        playerInputActions.Player.Skill1.performed -= OnSkill1;
        playerInputActions.Player.Skill2.performed -= OnSkill2;
        playerInputActions.Player.Skill3.performed -= OnSkill3;
        playerInputActions.Player.Skill4.performed -= OnSkill4;

        playerInputActions.Player.ItemUse1.performed -= OnItem1;
        playerInputActions.Player.ItemUse2.performed -= OnItem2;
        playerInputActions.Player.ItemUse3.performed -= OnItem3;
        playerInputActions.Player.ItemUse4.performed -= OnItem4;
    }

    private void OnSkill1(InputAction.CallbackContext ctx) => OnSkillUse?.Invoke(0, playerskillController);
    private void OnSkill2(InputAction.CallbackContext ctx) => OnSkillUse?.Invoke(1, playerskillController);
    private void OnSkill3(InputAction.CallbackContext ctx) => OnSkillUse?.Invoke(2, playerskillController);
    private void OnSkill4(InputAction.CallbackContext ctx) => OnSkillUse?.Invoke(3, playerskillController);
    private void OnItem1(InputAction.CallbackContext ctx) => OnItemlUse?.Invoke(0, playerItemHandler);
    private void OnItem2(InputAction.CallbackContext ctx) => OnItemlUse?.Invoke(1, playerItemHandler);
    private void OnItem3(InputAction.CallbackContext ctx) => OnItemlUse?.Invoke(2, playerItemHandler);
    private void OnItem4(InputAction.CallbackContext ctx) => OnItemlUse?.Invoke(3, playerItemHandler);

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!player.IsAttacking)
        {
            playerController.ComboAttack();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        CurrentInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        CurrentInput = Vector2.zero;
    }

    public void SetActionEnabled(bool enabled)
    {
        if (enabled)
            playerInputActions.Player.Enable();
        else
            playerInputActions.Player.Disable();
    }

    private void OnEnable()
    {
        RegisterBinding();
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        UnregisterBinding();
        playerInputActions.Player.Disable();
    }

}
