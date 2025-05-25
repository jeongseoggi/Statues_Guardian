using UnityEngine;

/// <summary>
/// 상태머신 인터페이스
/// </summary>
public interface IStateMachine
{
    State CurState { get; }
    Animator Animator { get; }
    object GetOwner();
    void SetState(STATE state);
}

/// <summary>
/// 맞았을 때 호출되는 인터페이스
/// </summary>
public interface IHitable
{
    void Hit(float atk);
}

/// <summary>
/// 공격 했을 때 인터페이스
/// </summary>
public interface IAttackable
{
    void Attack(IHitable target);
}

/// <summary>
/// 풀링 인터페이스
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IObjectPool<T>
{
    void Init(int size);
    T SpawnPool();
    void ReturnPool(T poolObject);
}

public interface IUseable
{
    void Upgrade(UpgradeType upgradeType, int useCount = 1);
    void Heal(float amount, HealType healType);
    float GetMaxHp();
    float GetMaxMp();
}

public interface ISkillUable
{
    void SpeedUpApply(float speedUpAmount, float duration, int mpCost);
    void AttackUpApply(float attackUpAmount, float duration, int mpCost);
    void AoEApply(float duration, int mpCost);
    void DotDamageApply(float curseDamage, float duration, int mpCost);
}

public interface IItemUseStrategy
{
    void Use(IUseable user, ItemData itemData, int useCount = 1);
}

public interface ISkillUseStrategy
{
    void SkillUse(ISkillUable user, SkillData skillData);
}
