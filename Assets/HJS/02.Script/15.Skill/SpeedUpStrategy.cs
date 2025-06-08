using System.Collections;
using UnityEngine;

/// <summary>
/// 이속 관련 스킬 전략
/// </summary>
public class SpeedUpStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        float increase = skillData.increase + 
            (skillData.increasePerLevel * GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName]);

        float resetVal = user.ApplyBuff(BuffType.MoveSpeedUp, increase, skillData.mpCost);
        user.RunCoroutine(BuffTime(skillData.duration, resetVal, user));
    }

    private IEnumerator BuffTime(float duration, float returnval, ISkillUable user)
    {
        yield return new WaitForSeconds(duration);
        user.ReturnBuffValue(BuffType.MoveSpeedUp, returnval);
    }
}
