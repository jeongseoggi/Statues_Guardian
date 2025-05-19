using DG.Tweening;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    readonly string[] attackComboNameArray = new string[] { "Attack01", "Attack02", "Attack03" };
    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        player = sm.GetOwner() as Player;
    }

    public override void Enter()
    {
        player.state = STATE.ATTACK;
        SetAnimationPlay();
        player.IsAttacking = true;
       
    }

    public override void Exit()
    {
        // player.weapon.weaponCol.enabled = false;
    }
    public override void Update()
    {
        
    }

    public void SetAnimationPlay()
    { 
        int comboCount = player.GetCombo();
        switch(comboCount)
        {
            case 0:
                sm.Animator.SetTrigger("AttackCom01");
                break;
            case 1:
                sm.Animator.SetTrigger("AttackCom02");
                break;
            case 2:
                sm.Animator.SetTrigger("AttackCom03");
                break;
        }

        player.WaitAnimAction?.Invoke(attackComboNameArray[comboCount]);
            
    }
}
