using SimpleJSON;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowSlot : MonoBehaviour
{
    [SerializeField] Image skillIcon;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillLevelTMP;
    [SerializeField] Button levelUpButton;
    [SerializeField] private int skillMasterLv;
    [SerializeField] private int curSkillLevel;

    public void SkillSlotSet(Sprite skillIcon, string skillName, int skillLevel, int skillMasterLv)
    {
        this.skillIcon.sprite = skillIcon;
        this.skillName.text = skillName;
        this.curSkillLevel = skillLevel;
        skillLevelTMP.text = skillLevel.ToString();
        this.skillMasterLv = skillMasterLv;
    }

    public void UpGradeSkill()
    {
        if (GameManager.Instance.PlayerData.SkillPoints <= 0)
            return;

        if (skillMasterLv <= curSkillLevel)
            return;

        levelUpButton.interactable = false;
        StartCoroutine(SkillLevelUp());
    }

    IEnumerator SkillLevelUp()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", GameManager.Instance.PlayerData.ID);
        form.AddField("skillName", skillName.text);
        yield return StartCoroutine(DataManager.GameConnect("playerSkill/levelUp", form, data =>
        {
            JSONNode json = JSONNode.Parse(data);
            Debug.Log(json);
            if (json["success"].AsBool)
            {
                levelUpButton.interactable = true;
                curSkillLevel = json["skill"]["skill_level"].AsInt;
                this.skillLevelTMP.text = curSkillLevel.ToString();
                GameManager.Instance.PlayerData.SkillPoints = json["remainingSkillpoints"].AsInt;
            }

        }));
    }
}
