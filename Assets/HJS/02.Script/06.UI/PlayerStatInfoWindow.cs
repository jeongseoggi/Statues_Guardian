using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStatInfoWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private GameObject playerStatInfoMainObject;
    [SerializeField] private RectTransform playerStatInfoWindowPanel;
    [SerializeField] private TextMeshProUGUI[] statTexts;
    private Vector2 offset;
    private Player player;

    public void ActivePlayerStatInfoWindow()
    {
        playerStatInfoMainObject.SetActive(!playerStatInfoMainObject.activeSelf);
        if (playerStatInfoMainObject.activeSelf)
        {
            UIManager.Instance.openUIStack.Push(playerStatInfoMainObject);
            SettingInfo();
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

    #region 인터페이스 구현부
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 마우스 위치와 패널 좌상단 사이의 거리 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            playerStatInfoWindowPanel,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            playerStatInfoWindowPanel.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            playerStatInfoWindowPanel.localPosition = localPoint - offset;
        }
    }
    #endregion
}

