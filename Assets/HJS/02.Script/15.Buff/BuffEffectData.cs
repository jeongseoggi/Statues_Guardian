using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BuffEffectData
{
    public abstract void UseEffect(IBuffUsable user, float duration, float increse, string buffName);
    public virtual IEnumerator StatBuff(IBuffUsable user, float duration, float increase, BuffType buffType, string buffName)
    {
        if(buffType == BuffType.DefDown)
        {
            increase = increase * -1;
        }

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
        user.ReturnBuffValue(buffType, increase);
        BuffNotifier.NotifyBuffRemoved(buffName);
    }
    public virtual IEnumerator TimeBuff(IBuffUsable user, float duration, float increase, BuffType buffType, string buffName)
    {
        float returnSpeedValue = 0;
        float retrurnAtkSpeedValue = 0;
        foreach (Monster mon in SpawnManager.instance.spawnMonsterList)
        {
            returnSpeedValue = mon.Speed;
            retrurnAtkSpeedValue = mon.AttackSpeed;

            mon.AttackSpeed -= increase;
            mon.Speed -= increase;
        }
         
        yield return new WaitForSeconds(duration);
        foreach (Monster mon in SpawnManager.instance.spawnMonsterList)
        {
            mon.Speed = returnSpeedValue;
            mon.AttackSpeed = retrurnAtkSpeedValue;
        }

        BuffNotifier.NotifyBuffRemoved(buffName);
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
        BuffNotifier.NotifyBuffRemoved(buffName);
    }
    public virtual IEnumerator InfinityManaBuff(IBuffUsable user, float duration, BuffType buffType, string buffName)
    {
        user.ApplyBuff(BuffType.InfinityMana);
        if (duration <= 0)
        {
            int curWave = WaveManager.curWave;
            yield return new WaitUntil(() => curWave < WaveManager.curWave);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        user.ReturnBuffValue(buffType);
        BuffNotifier.NotifyBuffRemoved(buffName);
    }
}


