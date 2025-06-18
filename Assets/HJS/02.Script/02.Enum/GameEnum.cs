using UnityEngine;

/// <summary>
///  캐릭터 상태
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
/// 몬스터 타입
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
/// 게임의 현재 상태 
/// </summary>
public enum GameState
{
    Play,
    Loading,
    Paused,
    Wait
}

/// <summary>
/// 아이템 타입
/// </summary>
public enum ItemType
{
    Buff,
    Heal,
    Gamble,
    Upgrade,
    Gold
}

/// <summary>
/// 회복타입
/// </summary>
public enum HealType
{
    HP,
    MP
}

/// <summary>
/// 업그레이드 타입
/// </summary>
public enum UpgradeType
{
    Atk,
    Def,
    SP
}

/// <summary>
/// 스킬 타입
/// </summary>
public enum SkillType
{
    Passive,
    Active
}


/// <summary>
/// 데미지 타입
/// </summary>
public enum DamageType
{
    SpeedUp,
    Dot,
    AoE,
    AttackUp
}

public enum BuffType
{
    AttackUp,
    DefUp,
    AtkSpeed,
    MoveSpeedUp,
    Time,
    DefDown,
    InfinityMana
}

public enum EffectType
{
    HealEffect,
    DotEffect,
    AoeEffect
}