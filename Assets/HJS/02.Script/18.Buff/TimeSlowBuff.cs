public class TimeSlowBuff : BuffEffectData
{
    public override void UseEffect(IBuffUsable user, float duration, float increse, string buffName)
    {
        user.RunCoroutine(TimeBuff(user, duration, increse, BuffType.Time, buffName));
    }
}

