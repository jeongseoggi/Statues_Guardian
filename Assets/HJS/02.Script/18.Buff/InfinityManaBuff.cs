public class InfinityManaBuff : BuffEffectData
{
    public override void UseEffect(IBuffUsable user, float duration, float increse, string buffName)
    {
        user.RunCoroutine(InfinityManaBuff(user, duration, BuffType.InfinityMana, buffName));
    }
}

