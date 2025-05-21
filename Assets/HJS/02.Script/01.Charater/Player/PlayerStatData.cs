using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatData
{
    float hp;
    float mp;
    float maxHp;
    float maxMp;
    float atk;
    float def;
    float speed;

    public float Hp { get => hp; set => hp = value; }
    public float Mp { get => mp; set => mp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxMp { get => maxMp;set => maxMp = value; }
    public float Atk { get => atk; set => atk = value; }
    public float Def { get => def; set => def = value; }
    public float Speed { get => speed; set => speed = value; }
}
