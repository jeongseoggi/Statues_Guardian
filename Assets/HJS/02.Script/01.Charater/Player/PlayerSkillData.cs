using System.Collections.Generic;

public class PlayerSkillData
{
    public Dictionary<string, int> playerSkillLevelDic;

    public PlayerSkillData()
    {
        playerSkillLevelDic = new Dictionary<string, int>();
    }

    /// <summary>
    /// 플레이어의 스킬 정보가 없을 때 스킬 정보 저장(모든 스킬 1로 값 초기화)
    /// </summary>
    public void AddNewSkillData()
    {
        foreach(SkillData skillData in DataManager.Instance.SkillDataBase.skillDatas)
        {
            playerSkillLevelDic.Add(skillData.skillName, 1);
        }
    }
}