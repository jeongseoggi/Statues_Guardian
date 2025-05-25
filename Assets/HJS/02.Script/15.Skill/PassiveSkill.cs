using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PassiveSkill", menuName = "Skill/PassiveSkillData")]
public class PassiveSkill : SkillData
{
    public override void SkillUse(ISkillUable user)
    {
        skillStarategy?.SkillUse(user, this);
    }
}
