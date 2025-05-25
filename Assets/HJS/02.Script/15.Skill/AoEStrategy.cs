using UnityEngine;

public class AoEStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        user.AoEApply(skillData.duration, skillData.mpCost);
    }
}
