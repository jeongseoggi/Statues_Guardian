public class AttackUpBuff : BuffEffectData
{
    public override void UseEffect(IBuffUsable user, float duration, float increse, string buffName)
    {
        user.RunCoroutine(InchantBuff(user, duration, increse, BuffType.AttackUp, buffName));
    }
}


