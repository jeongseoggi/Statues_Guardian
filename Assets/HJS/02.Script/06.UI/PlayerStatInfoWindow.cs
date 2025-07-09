using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStatInfoWindow : ActiveUI
{ 
    //[SerializeField] private GameObject playerStatInfoMainObject;
    //[SerializeField] private RectTransform playerStatInfoWindowPanel;


    [SerializeField] private TextMeshProUGUI[] statTexts;
    private Player player;

    public override void ActiveWindow()
    {
        activeWindowMainObject.SetActive(!activeWindowMainObject.activeSelf);
        if (activeWindowMainObject.activeSelf)
        {
            UIManager.Instance.openUIStack.Push(activeWindowMainObject);
            SettingInfo();
        }
        else
        {
            UIManager.Instance.RemoveUI(activeWindowMainObject);
        }
    }

    public void SettingInfo()
    {
        player = player == null ? GameManager.Instance.GetPlayer() : player;

        statTexts[0].text = "HP : " + player.Hp + " / " + player.MaxHp;
        statTexts[1].text = "MP : " + player.Mp + " / " + player.MaxMp;
        statTexts[2].text = "ATK : " + player.Atk;
        statTexts[3].text = "DEF : " + player.Def;
        statTexts[4].text = "SPEED : " + player.Speed;
        statTexts[5].text = "ATK SPEED : " + player.AttackSpeed;
        statTexts[6].text = "GOLD : " + GameManager.Instance.PlayerData.Gold;
        statTexts[7].text = "CLEAR STAGE : " + GameManager.Instance.PlayerData.Stage;

    }
}

