using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class AttackUpStrategy : ISkillUseStrategy
{

    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        // 스킬 레벨에 따른 증가량 계산
        float increase = skillData.increase + 
            (skillData.increasePerLevel * (GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName] - 1));

        user.ApplyBuff(BuffType.AttackUp, increase, skillData.mpCost);
        user.RunCoroutine(BuffTime(skillData.duration, increase, user));
    }

    //버프 시간 코루틴
    private IEnumerator BuffTime(float duration, float increase, ISkillUable user)
    {
        yield return new WaitForSeconds(duration);
        user.ReturnBuffValue(BuffType.AttackUp, increase);
    }
}
