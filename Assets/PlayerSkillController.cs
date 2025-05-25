using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSkillController : MonoBehaviour, ISkillUable
{
    [SerializeField] Player player;
    public void AttackUpApply(float attackUpAmount, float duration, int mpCost)
    {
        player.Mp -= mpCost;
        float originAttackVal = player.Atk;
        player.Atk *= attackUpAmount;
        StartCoroutine(InCreaseCor(originAttackVal, duration, () => { player.Atk = originAttackVal; }));
    }

    public void DotDamageApply(float curseDamage, float duration, int mpCost)
    {
        throw new System.NotImplementedException();
    }

    public void SpeedUpApply(float speedUpAmount, float duration, int mpCost)
    {
        player.Mp -= mpCost;
        float originSpeed = player.Speed;
        player.Speed *= speedUpAmount;
        StartCoroutine(InCreaseCor(originSpeed, duration, () => { player.Speed = originSpeed; }));
    }

    public void AoEApply(float duration, int mpCost)
    {

    }


    IEnumerator InCreaseCor(float origin, float duration, UnityAction resetAction)
    {
        float time = 0;
        while(duration > time)
        {
            time += Time.deltaTime;
            yield return null;
        }
        resetAction?.Invoke();
    }

    /// <summary>
    /// 테스트 코드
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SkillManager.Instance.UseSkill(0, this);
        if (Input.GetKeyDown(KeyCode.LeftControl))
            SkillManager.Instance.UseSkill(3, this);
    }
}
