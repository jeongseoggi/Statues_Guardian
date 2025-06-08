using System;
using UnityEngine;
/// <summary>
/// 플레이어 인풋 관련 처리 클래스
/// </summary>
public class PlayerActionInput : MonoBehaviour
{
    #region private
    [SerializeField] private PlayerSkillController  playerskillController;
    [SerializeField] private Player                 player;
    [SerializeField] private PlayerItemHandler      playerItemHandler;
                     private PlayerInputActions     playerInputActions;
    #endregion

    public static event Action<int, ISkillUable> OnSkillUse;
    public static event Action<int, IUseable> OnItemlUse;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        RegisterBinding();
    }

    private void RegisterBinding()
    {
        playerInputActions.Player.Skill1.performed += ctx => OnSkillUse?.Invoke(0, playerskillController);
        playerInputActions.Player.Skill2.performed += ctx => OnSkillUse?.Invoke(1, playerskillController);
        playerInputActions.Player.Skill3.performed += ctx => OnSkillUse?.Invoke(2, playerskillController);
        playerInputActions.Player.Skill4.performed += ctx => OnSkillUse?.Invoke(3, playerskillController);

        playerInputActions.Player.ItemUse1.performed += ctx => OnItemlUse?.Invoke(0, playerItemHandler);
        playerInputActions.Player.ItemUse2.performed += ctx => OnItemlUse?.Invoke(1, playerItemHandler);
        playerInputActions.Player.ItemUse3.performed += ctx => OnItemlUse?.Invoke(2, playerItemHandler);
        playerInputActions.Player.ItemUse4.performed += ctx => OnItemlUse?.Invoke(3, playerItemHandler);
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }


}
