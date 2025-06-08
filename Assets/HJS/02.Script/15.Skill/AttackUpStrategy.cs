using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class AttackUpStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        float increase = skillData.increase + 
            (skillData.increasePerLevel * GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName]);

        float resetVal = user.ApplyBuff(BuffType.AttackUp, increase, skillData.mpCost);
        user.RunCoroutine(BuffTime(skillData.duration, resetVal, user));
    }
    private IEnumerator BuffTime(float duration, float returnval, ISkillUable user)
    {
        yield return new WaitForSeconds(duration);
        user.ReturnBuffValue(BuffType.AttackUp, returnval);
    }
}
