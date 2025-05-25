using UnityEngine;

public class DotDamageStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        user.DotDamageApply(skillData.damage, skillData.duration, skillData.mpCost);
    }
}
