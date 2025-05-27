using NUnit.Framework;
using System.Transactions;
using UnityEngine;

public class Weapon : MonoBehaviour, IAttackable
{
    public CircleCollider2D weaponCol;
    public float damage;
    public Character owner;
    private int[] comboDmgData = new int[] { 3000, 5, 10 };

    public void Attack(IHitable target)
    {
        target.Hit(damage);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //몬스터끼리 데미지 주는 현상 막기
        if(owner is Monster && other.GetComponent<Monster>() != null)
        {
            return;
        }

        if (owner is Player && other.GetComponent<StageObject>() != null)
            return;

        if (other.GetComponent<IHitable>() != null)
        {
            Attack(other.GetComponent<IHitable>());
        }
    }

    /// <summary>
    /// 무기 주인
    /// </summary>
    /// <param name="charater"></param>
    public void SetOwner(Character charater)
    {
        this.owner = charater;
    }

    /// <summary>
    /// 콤보 당 무기 데미지 추가
    /// </summary>
    /// <param name="combo"></param>
    public void SetComboDmg(int combo)
    {
        damage = comboDmgData[combo];
    }
}
