using UnityEngine;

public class DotDamageStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        float damage = skillData.damage + 
            (skillData.damagePerLevel * GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName]);
        user.DotDamageApply(damage, skillData.duration, skillData.mpCost);
    }
}
