using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BuffEffectData
{
    public abstract void UseEffect(IBuffUsable user, float duration, float increse, string buffName);
    public virtual IEnumerator InchantBuff(IBuffUsable user, float duration, float increase, BuffType buffType, string buffName)
    {
        user.ApplyBuff(buffType, increase);
        if (duration <= 0)
        {
            int curWave = WaveManager.curWave;
            yield return new WaitUntil(() => curWave < WaveManager.curWave);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        user.ReturnBuffValue(buffType, user.GetReturnValue(buffType));
        UIManager.Instance.buffManager.UnActiveBuff(buffName);
    }
    public virtual IEnumerator SpeedBuff(IBuffUsable user, float duration, float increase, BuffType buffType, string buffName)
    {
        user.ApplyBuff(buffType, increase);
        if (duration <= 0)
        {
            int curWave = WaveManager.curWave;
            yield return new WaitUntil(() => curWave < WaveManager.curWave);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        user.ReturnBuffValue(buffType, user.GetReturnValue(buffType));
        UIManager.Instance.buffManager.UnActiveBuff(buffName);
    }
    public virtual IEnumerator TimeBuff(IBuffUsable user, float duration, float increase, BuffType buffType, string buffName)
    {
        float returnValue = 0;
        foreach (Monster mon in SpawnManager.instance.spawnMonsterList)
        {
            returnValue = mon.Speed;
            mon.Speed -= increase;
        }
         
        yield return new WaitForSeconds(duration);
        foreach (Monster mon in SpawnManager.instance.spawnMonsterList)
        {
            mon.Speed = returnValue;
        }

        UIManager.Instance.buffManager.UnActiveBuff(buffName);
    }
    public virtual IEnumerator DownBuff(IBuffUsable user, float duration, float increase, BuffType buffType, string buffName)
    {
        user.ApplyBuff(buffType, increase);
        
        if (duration <= 0)
        {
            int curWave = WaveManager.curWave;
            yield return new WaitUntil(() => curWave < WaveManager.curWave);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        user.ReturnBuffValue(buffType, user.GetReturnValue(buffType));
        UIManager.Instance.buffManager.UnActiveBuff(buffName);
    }
    public virtual IEnumerator InfinityManaBuff(IBuffUsable user, float duration, float increase, BuffType buffType, string buffName)
    {
        user.ApplyBuff(buffType, increase);
        if (duration <= 0)
        {
            int curWave = WaveManager.curWave;
            yield return new WaitUntil(() => curWave < WaveManager.curWave);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        user.ReturnBuffValue(buffType, user.GetReturnValue(buffType));
        UIManager.Instance.buffManager.UnActiveBuff(buffName);
    }
}


