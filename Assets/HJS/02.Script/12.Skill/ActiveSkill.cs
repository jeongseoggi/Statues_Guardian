using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ActiveSkill", menuName = "Skill/ActiveSkillData")]
public class ActiveSkill : SkillData
{
    public override void SkillUse(ISkillUable user)
    {
        skillStarategy?.SkillUse(user, this);
    }
}
