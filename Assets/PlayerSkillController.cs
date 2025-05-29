using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSkillController : MonoBehaviour, ISkillUable
{
    [SerializeField] Player player;


    public float ApplyBuff(BuffType buffType, float amount, int mpCost)
    {
        float originValue = 0;
        switch (buffType)
        {
            case BuffType.Attack:
                originValue = player.Atk;
                player.Atk *= amount;
                break;
            case BuffType.Speed:
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


    /// <summary>
    /// 테스트 코드
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SkillManager.Instance.UseSkill(0, this);
        if (Input.GetKeyDown(KeyCode.E))
            SkillManager.Instance.UseSkill(1, this);
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SkillManager.Instance.UseSkill(2, this);
        if (Input.GetKeyDown(KeyCode.LeftControl))
            SkillManager.Instance.UseSkill(3, this);
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
            case BuffType.Attack:
                player.Atk = returnVal;
                break;
            case BuffType.Speed:
                player.Speed = returnVal;
                break;
        }
    }

    public Player GetPlayer()
    {
        return player;
    }
}
