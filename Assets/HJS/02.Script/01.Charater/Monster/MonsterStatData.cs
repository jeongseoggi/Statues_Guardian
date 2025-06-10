using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStatData", menuName = "Scriptable Objects/MonsterStatData")]
public class MonsterStatData : ScriptableObject
{
    public readonly Vector2 attackRangeOriginSize = new Vector2(0.2097399f, 0.1963018f);
    public readonly Vector2 attackRangeOriginOffset = new Vector2(0.1317067f, 0f);

    public MonsterType monsterType;
    public float maxHealth;
    public float attackPower;
    public float defense;
    public float moveSpeed;
    public float atkSpeed;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public Vector2 attackRangeSize;
    public Vector2 attackRangeOffset;
}  
