using Mono.Cecil;
using NUnit.Framework;
using System;
using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour, IAttackable
{
    public CircleCollider2D weaponCol;
    public float damage;
    public Character owner;
    private int[] comboDmgData = new int[] { 3, 5, 10 };
    public event Action<Player,Monster> OnDotDamage;
    private IHitable target;
    private float comboDmg;

    public void Attack(IHitable target)
    {
        if(owner is Player player && player.isDotActive)
        {
            if (target is Monster monster && !monster.isDotState)
            {
                OnDotDamage?.Invoke(player, monster);
            }

        }
        target.Hit(damage + comboDmg);
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
            target = other.GetComponent<IHitable>();
            Attack(target);
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
        comboDmg = comboDmgData[combo];
    }

    public IEnumerator DotDamage(Player player, float damage, float duration, Monster target)
    {
        float time = 0;
        GameObject effectObject = EffectPoolManager.Instance?.GetEffect(EffectType.DotEffect, target.gameObject.transform);
        while(time < duration) 
        {
            yield return new WaitForSeconds(0.5f);
            time += 0.5f;
#if UNITY_EDITOR
            Debug.Log("도트 데미지 입히는 중");
#endif
            target.Hit(damage);
        }
        player.isDotActive = false;
        target.isDotState = false;
        EffectPoolManager.Instance?.ReturnEffect(EffectType.DotEffect, effectObject);
    }


}
