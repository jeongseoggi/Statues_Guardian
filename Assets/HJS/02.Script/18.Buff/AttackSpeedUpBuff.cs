public class AttackSpeedUpBuff : BuffEffectData
{
    public override void UseEffect(IBuffUsable user, float duration, float increse, string buffName)
    {
        user.RunCoroutine(StatBuff(user, duration, increse, BuffType.AtkSpeed, buffName));
    }
}

