using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStatData", menuName = "Scriptable Objects/MonsterStatData")]
public class MonsterStatData : ScriptableObject
{
    public readonly Vector2 attackRangeOriginSize = new Vector2(0.1263266f, 0.1963018f);
    public readonly Vector2 attackRangeOriginOffset = new Vector2(0.09f, 0.5136464f);

    public MonsterType monsterType;
    public float maxHealth;
    public float attackPower;
    public float defense;
    public float moveSpeed;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public Vector2 attackRangeSize;
    public Vector2 attackRangeOffset;
}  
