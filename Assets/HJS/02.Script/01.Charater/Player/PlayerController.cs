using System;
using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    #region ����
    [SerializeField] Animator animator; // �ִϸ�����
    [SerializeField] Player player; 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Camera cam;

    // private
    private Vector2 currentInput;
    private Vector2 moveVelocity;

    //public
    public int attackCombo;
    public int maxCombo = 2; // �ִ� �޺� �ܰ�
    public float comboResetTime = 1.5f; // �޺� �ʱ�ȭ���� ��� �ð�

    private bool isComboPossible = false;
    private float comboTimer;
    #endregion

    #region ������Ƽ
    public Animator Animator => animator;
    #endregion

    void Start()
    {
        player.StateMachine.SetAnimator(animator);
        player.StateMachine.SetState(STATE.IDLE);

        player.getCombo += () => { return attackCombo; };
    }


    void Update()
    {
        player.StateMachine.Update();
        moveVelocity = currentInput;

        if (Input.GetMouseButtonDown(0) && !player.IsAttacking)
        {
            ComboAttack();
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ActiveInventory();
        }

        if (isComboPossible)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboResetTime)
            {
                ResetCombo();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            UIManager.Instance.quickSlotManager.quickSlots[3].ItemData?.Use(player);
        }


        if (moveVelocity.x != 0 || moveVelocity.y != 0)
        {
            player.StateMachine.SetState(STATE.MOVE);
        }
        else
        {
            player.StateMachine.SetState(STATE.IDLE);
        }
        RotateToMouse();
    }

    void FixedUpdate()
    {
         rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime * player.Speed);
    }

    void OnMove(InputValue inputValue)
    {
        currentInput = inputValue.Get<Vector2>();
    }

    void RotateToMouse()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        player.SetFilpX(dir);
    }

    void ComboAttack()
    {
        player.StateMachine.SetState(STATE.ATTACK);

        attackCombo++;
        if (attackCombo > maxCombo)
        {
            attackCombo = 0; // �ٽ� 1�޺�����
        }



        isComboPossible = true;
        comboTimer = 0f; // �޺� Ÿ�̸� ����
    }

    void ResetCombo()
    {
        attackCombo = 0;
        isComboPossible = false;
        comboTimer = 0f;
    }
}