using Newtonsoft.Json;
using System;

public class PlayerData
{
    public PlayerData(int id, int level, string nickName, int curStage, int gold, int skillPoints) 
    {
        this.id = id;
        this.level = level; 
        this.nickName = nickName;
        this.curStage = curStage;
        this.gold = gold;
        this.skillPoints = skillPoints;
    }

    int id;
    int level;
    string nickName;
    int curStage;
    int gold;
    int skillPoints;

    [JsonProperty("id")]
    public int ID { get => id; set => id = value; }
    [JsonProperty("level")]
    public int Level { get => level; set => level = value; }
    [JsonProperty("name")]
    public string NickName { get => nickName; set => nickName = value; }
    [JsonProperty("stage")]
    public int Stage { get => curStage; set=> curStage = value; }
    [JsonProperty("gold")]
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            OnGoldValueChanged?.Invoke(gold);
        }
    }
    [JsonProperty("skillPoints")]
    public int SkillPoints 
    { 
        get => skillPoints;
        set 
        {
           skillPoints = value;
           OnSkillPointValueChanged?.Invoke(skillPoints);
        }
    }

    public event Action<int> OnGoldValueChanged;
    public event Action<int> OnSkillPointValueChanged;
    public int GetCurStage() => curStage;
    public int GetMyGold() => gold;

}
