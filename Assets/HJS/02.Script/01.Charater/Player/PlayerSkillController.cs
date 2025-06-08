using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSkillController : MonoBehaviour, ISkillUable
{
    [SerializeField] Player player;


    public float ApplyBuff(BuffType buffType, float amount, int mpCost)
    {
        float originValue = 0;
        switch (buffType)
        {
            case BuffType.AttackUp:
                originValue = player.Atk;
                player.Atk *= amount;
                break;
            case BuffType.MoveSpeedUp:
                originValue = player.Speed;
                player.Speed *= amount;
                break;
        }

        player.Mp -= mpCost;
        return originValue;
    }


    public void DotDamageApply(float curseDamage, float duration, int mpCost)
    {
        player.isDotActive = true;
        player.Weapon.OnDotDamage += (player, monster) =>
        {
            player.Weapon.StartCoroutine(player.Weapon.DotDamage(player, curseDamage, duration, monster));
        };
        player.Mp -= mpCost ;
    }

    public void AoEApply(float duration, int mpCost)
    {
        player.Mp -= mpCost;
    }


    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void ReturnBuffValue(BuffType buffType, float returnVal)
    {
        switch(buffType) 
        {
            case BuffType.AttackUp:
                player.Atk = returnVal;
                break;
            case BuffType.MoveSpeedUp:
                player.Speed = returnVal;
                break;
        }
    }

    public Player GetPlayer()
    {
        return player;
    }
}
