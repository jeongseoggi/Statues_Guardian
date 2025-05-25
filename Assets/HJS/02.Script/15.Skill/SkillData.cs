using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public abstract class SkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public int coolTime;
    public int mpCost;
    public float damage;
    public float duration;
    public float increase;
    public string spriteName;
    public string assetName;
    public SkillType skillType;
    public DamageType damageType;
    public ISkillUseStrategy skillStarategy;

    public abstract void SkillUse(ISkillUable user);

    public virtual void Init(string skillName, string skillDesc, int coolTime, int mpCost, float damage, float duration,
        float increase ,string spriteName, string assetName, SkillType skillType, DamageType damageType)
    {
        this.skillName = skillName;
        this.skillDescription = skillDesc;
        this.coolTime = coolTime;
        this.mpCost = mpCost;
        this.damage = damage;
        this.duration = duration;
        this.increase = increase;
        this.spriteName = spriteName;
        this.assetName = assetName;
        this.skillType = skillType;
        this.damageType = damageType;
    }
}
