using UnityEngine;

public static class PlayerStatCalculator
{
    public static float baseSpeed = 3;
    public static float baseAttack = 30;

    public static StatModifier SpeedMod = new();

    public static float Speed => SpeedMod.Apply(baseSpeed);
    public static float AtkMod => SpeedMod.Apply(baseAttack);

    public static void AddSpeed(float value) => SpeedMod.Additive += value;
    public static void MulSpeed(float value) => SpeedMod.Multiplicative *= value;

    public static void RemoveSpeed(float value, bool isMul)
    {
        if (isMul) SpeedMod.Multiplicative /= value;
        else SpeedMod.Additive -= value;
    }

}

public class StatModifier
{
    public float Additive = 0f;
    public float Multiplicative = 1f;

    public float Apply(float baseValue)
    {
        return (baseValue + Additive) * Multiplicative;
    }
}

