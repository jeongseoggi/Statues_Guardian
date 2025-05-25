using UnityEngine;

public class AttackUpStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        user.AttackUpApply(skillData.increase, skillData.duration, skillData.mpCost);
    }
}
