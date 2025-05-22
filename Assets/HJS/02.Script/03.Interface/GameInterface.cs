using UnityEngine;

/// <summary>
/// ���¸ӽ� �������̽�
/// </summary>
public interface IStateMachine
{
    State CurState { get; }
    Animator Animator { get; }
    object GetOwner();
    void SetState(STATE state);
}

/// <summary>
/// �¾��� �� ȣ��Ǵ� �������̽�
/// </summary>
public interface IHitable
{
    void Hit(float atk);
}

/// <summary>
/// ���� ���� �� �������̽�
/// </summary>
public interface IAttackable
{
    void Attack(IHitable target);
}

/// <summary>
/// Ǯ�� �������̽�
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

public interface IItemUseStrategy
{
    void Use(IUseable user, ItemData itemData, int useCount = 1);
}
