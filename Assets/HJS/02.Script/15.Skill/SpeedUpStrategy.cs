using UnityEngine;

/// <summary>
/// 이속 관련 스킬 전략
/// </summary>
public class SpeedUpStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        user.SpeedUpApply(skillData.increase, skillData.duration, skillData.mpCost);
    }
}
