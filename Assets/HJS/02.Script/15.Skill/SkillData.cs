using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public abstract class SkillData : ScriptableObject
{
    public string skillName;                    //스킬 이름
    public string skillDescription;             //스킬 설명
    public int coolTime;                        //스킬 쿨타임
    public int mpCost;                          //스킬 마나 소모량
    public float damage;                        //스킬 데미지
    public float duration;                      //스킬 지속시간
    public float increase;                      //스킬 효과 증가량
    public string spriteName;                   //스킬 이미지 이름
    public string assetName;                    //스킬 에셋 이름
    public float damagePerLevel;                //스킬 레벨 당 데미지 증가량
    public float increasePerLevel;              //스킬 레벨 당 효과 증가량
    public int skillMasterLevel;                //스킬 마스터 레벨
    public SkillType skillType;                 //스킬 타입
    public DamageType damageType;               //스킬 데미지 타입
    public ISkillUseStrategy skillStarategy;    //스킬 전략

    public abstract void SkillUse(ISkillUable user);

    public virtual void Init(string skillName, string skillDesc, int coolTime, int mpCost, float damage, float duration,
        float increase, float damagePerLevel, float increasePerLevel, int skillMasterLevel, 
        string spriteName, string assetName, SkillType skillType, DamageType damageType)
    {
        this.skillName = skillName;
        this.skillDescription = skillDesc;
        this.coolTime = coolTime;
        this.mpCost = mpCost;
        this.damage = damage;
        this.duration = duration;
        this.increase = increase;
        this.damagePerLevel = damagePerLevel;
        this.increasePerLevel = increasePerLevel;
        this.skillMasterLevel = skillMasterLevel;
        this.spriteName = spriteName;
        this.assetName = assetName;
        this.skillType = skillType;
        this.damageType = damageType;
    }
}
