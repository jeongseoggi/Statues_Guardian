using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : SingleTon<SkillManager>
{
    #region public
    public SkillSlot[]              skillSlot;
    public Dictionary<int, string>  skillSlotSaveDic;
    public Player                   player;
    #endregion

    private void Start()
    {
        skillSlotSaveDic = new Dictionary<int, string>();
        LoadSkillSlotData();
        player = GameManager.Instance.GetPlayer();
        player.OnCheckMana += CheckSlot;
        PlayerActionInput.OnSkillUse += UseSkill;
    }

    /// <summary>
    /// 스킬 퀵 슬롯 데이터 저장
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="skillData"></param>
    public void SaveSkillSlotData(int slotIndex, SkillData skillData)
    {
        if(skillData == null)
        {
            if(skillSlotSaveDic.ContainsKey(slotIndex))
            {
                skillSlotSaveDic.Remove(slotIndex);
            }
        }
        else
        {
            if(skillSlotSaveDic.ContainsKey(slotIndex))
            {
                skillSlotSaveDic[slotIndex] = skillData.assetName;
            }
            else
            {
                if(!string.IsNullOrEmpty(skillData.assetName))
                {
                    skillSlotSaveDic.Add(slotIndex, skillData.assetName);
                }
            }
        }

        string skillSlotJson = JsonConvert.SerializeObject(skillSlotSaveDic);
        PlayerPrefs.SetString("SkillSlotData", skillSlotJson);
    }

    /// <summary>
    /// 스킬 퀵 슬롯 데이터 로드
    /// </summary>
    public void LoadSkillSlotData()
    {
        if (!PlayerPrefs.HasKey("SkillSlotData"))
        {
            return;
        }
        else
        {
            string skillSlotJson = PlayerPrefs.GetString("SkillSlotData");
#if UNITY_EDITOR
            Debug.Log($"스킬 슬롯 데이터 로드 {skillSlotJson}");
#endif

            skillSlotSaveDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(skillSlotJson);

            foreach(var pair in skillSlotSaveDic.ToList())
            {
                int index = pair.Key;
                string assetName = pair.Value;

                skillSlot[index].SetSkillSlot(DataManager.Instance.GetSkillData(assetName));
            }
        }
    }

    /// <summary>
    /// 스킬 퀵 슬롯 스왑
    /// </summary>
    /// <param name="targetSlot"></param>
    /// <param name="swapSlot"></param>
    public void SkillSlotSwap(SkillSlot targetSlot, SkillSlot swapSlot)
    {
        SkillData tempSlot = targetSlot.SkillData;
        targetSlot.SetSkillSlot(swapSlot.SkillData);
        swapSlot.SetSkillSlot(tempSlot);
    }

    /// <summary>
    /// 스킬 사용 함수
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <param name="user"></param>
    public void UseSkill(int slotIndex, ISkillUable user)
    {
        if (skillSlot[slotIndex].IsCoolTime)
            return;

        if (skillSlot[slotIndex].IsNoMana)
        {
            UIManager.Instance.SetWarningText(GameString.NO_MANA);
            return;
        }
    
        skillSlot[slotIndex].SkillData?.SkillUse(user);
        skillSlot[slotIndex].StartCoolTime();
    }

    /// <summary>
    /// 슬롯 당 마나 체크
    /// </summary>
    public void CheckSlot()
    {
        foreach(var slot in skillSlot)
        {
            if(slot.SkillData != null && !string.IsNullOrEmpty(slot.SkillData.skillName))
            {
                slot.CheckSkillUse(player.Mp);
            }
        }

    }

    private void OnDestroy()
    {
        player.OnCheckMana -= CheckSlot;
    }
}
