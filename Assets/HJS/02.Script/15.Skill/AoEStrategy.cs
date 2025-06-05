using UnityEngine;

public class AoEStrategy : ISkillUseStrategy
{
    public Vector2 radius = new Vector2(6.5f, 6.5f);
    public void SkillUse(ISkillUable user, SkillData skillData)
    {

        Vector3 pos = user.GetPosition();

        Collider2D[] cols = Physics2D.OverlapBoxAll(pos, radius, 0);

        foreach (Collider2D col in cols)
        {
            if(col.TryGetComponent<Monster>(out Monster monster))
            {
                float damage = skillData.damage +
                    (skillData.damagePerLevel * GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName]);
                monster.aoeAction?.Invoke(damage, skillData.duration);
            }
        }

        GameObject effectObj = EffectPoolManager.Instance?.GetEffect(EffectType.AoeEffect);
        effectObj.transform.position = pos;
        user.AoEApply(skillData.duration, skillData.mpCost);
    }
}
