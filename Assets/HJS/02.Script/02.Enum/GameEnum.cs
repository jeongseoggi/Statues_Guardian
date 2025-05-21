using UnityEngine;

/// <summary>
///  ĳ���� ����
/// </summary>
public enum STATE
{
    IDLE,
    MOVE,
    ATTACK,
    HIT,
    TRAKING,
    DIE
}

/// <summary>
/// ���� Ÿ��
/// </summary>
public enum MonsterType
{
    Orc,
    Skeleton,
    ArmoredOrc,
    ArmoredSkeleton,
    OrcRider,
    SkeletonArcher,
    EliteOrc,
    GreatSwordSkeleton,
    Slime,
    Werebear,
    Werewolf
}

/// <summary>
/// ������ ���� ���� 
/// </summary>
public enum GameState
{
    Play,
    Loading,
    Paused
}

/// <summary>
/// ������ Ÿ��
/// </summary>
public enum ItemType
{
    Buff,
    Heal,
    Gamble,
    Upgrade
}

/// <summary>
/// ȸ��Ÿ��
/// </summary>
public enum HealType
{
    HP,
    MP
}

/// <summary>
/// ���׷��̵� Ÿ��
/// </summary>
public enum UpgradeType
{
    Atk,
    Def
}



