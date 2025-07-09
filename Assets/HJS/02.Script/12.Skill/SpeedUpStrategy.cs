using System.Collections;
using UnityEngine;

/// <summary>
/// 이속 관련 스킬 전략
/// </summary>
public class SpeedUpStrategy : ISkillUseStrategy
{
    public void SkillUse(ISkillUable user, SkillData skillData)
    {
        float increase = skillData.increase + 
            (skillData.increasePerLevel * (GameManager.Instance.PlayerSkillData.playerSkillLevelDic[skillData.skillName] - 1));

        Debug.Log(increase);
        user.ApplyBuff(BuffType.MoveSpeedUp, increase, skillData.mpCost);
        user.RunCoroutine(BuffTime(skillData.duration, increase, user));
        SoundManager.Instance.SpawnPool().Play(DataManager.Instance.GetAudioClip("SpeedUp"));
    }

    private IEnumerator BuffTime(float duration, float increase, ISkillUable user)
    {
        yield return new WaitForSeconds(duration);
        user.ReturnBuffValue(BuffType.MoveSpeedUp, increase);
    }
}
