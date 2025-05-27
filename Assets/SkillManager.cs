using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : SingleTon<SkillManager>
{
    public SkillSlot[] skillSlot;
    public Dictionary<int, string> skillSlotSaveDic = new Dictionary<int, string>();
    public Player player;

    private void Start()
    {
        LoadSkillSlotData();
        player = GameManager.Instance.GetPlayer();
        player.OnCheckMana += CheckSlot;
    }


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

    public void LoadSkillSlotData()
    {
        if (!PlayerPrefs.HasKey("SkillSlotData"))
        {
            return;
        }
        else
        {
            string skillSlotJson = PlayerPrefs.GetString("SkillSlotData");
            Debug.Log($"스킬 슬롯 데이터 로드 {skillSlotJson}");

            skillSlotSaveDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(skillSlotJson);

            foreach(var pair in skillSlotSaveDic.ToList())
            {
                int index = pair.Key;
                string assetName = pair.Value;

                skillSlot[index].SetSkillSlot(DataManager.Instance.GetSkillData(assetName));
            }
        }
    }

    public void SkillSlotSwap(SkillSlot targetSlot, SkillSlot swapSlot)
    {
        SkillData tempSlot = targetSlot.SkillData;
        targetSlot.SetSkillSlot(swapSlot.SkillData);
        swapSlot.SetSkillSlot(tempSlot);
    }


    public void UseSkill(int slotIndex, ISkillUable user)
    {
        if (skillSlot[slotIndex].IsCoolTime)
            return;

        if (skillSlot[slotIndex].IsNoMana)
        {
            UIManager.Instance.SetWarningText("마나가 부족합니다.");
            return;
        }
    
        skillSlot[slotIndex].SkillData?.SkillUse(user);
        skillSlot[slotIndex].StartCoolTime();
    }

    public void CheckSlot()
    {
        foreach(var slot in skillSlot)
        {
            slot.CheckSkillUse(player.Mp);
        }

    }

    private void OnDestroy()
    {
        player.OnCheckMana -= CheckSlot;
    }

    /// <summary>
    /// 테스트코드
    /// </summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (PlayerPrefs.HasKey("SkillSlotData"))
            {
                PlayerPrefs.DeleteKey("SkillSlotData");
            }
        }
    }
}
