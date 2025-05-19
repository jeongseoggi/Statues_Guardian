using NUnit.Framework;
using UnityEngine;

public class Weapon : MonoBehaviour, IAttackable
{
    public CircleCollider2D weaponCol;
    public float damage;
    public Character owner;
    private int[] comboDmgData = new int[] { 300, 500, 1000 };

    public void Attack(IHitable target)
    {
        target.Hit(damage);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //���ͳ��� ������ �ִ� ���� ����
        if(owner is Monster && other.GetComponent<Monster>() != null)
        {
            return;
        }

        if(other.GetComponent<IHitable>() != null)
        {
            Attack(other.GetComponent<IHitable>());
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="charater"></param>
    public void SetOwner(Character charater)
    {
        this.owner = charater;
    }

    /// <summary>
    /// �޺� �� ���� ������ �߰�
    /// </summary>
    /// <param name="combo"></param>
    public void SetComboDmg(int combo)
    {
        damage = comboDmgData[combo];
    }
}
