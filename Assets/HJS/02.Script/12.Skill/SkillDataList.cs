using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataList", menuName = "Skill/SkillDataList")]
public class SkillDataList : ScriptableObject
{
    public List<SkillData> skillDatas;

    public void Initalize()
    {
        skillDatas = new List<SkillData>();
    }
}
