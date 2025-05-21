using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using SimpleJSON;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : SingleTonDestory<ItemDetail>
{
    public ItemData selectItemData;

    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCountText;
    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI totalBuyPrice;
    public AmountInputPopup inputPopup;

    public int itemCount;

    public int ItemCount 
    { 
        get => itemCount; 
        set
        {
            itemCount = value;
            if(selectItemData != null)
                totalBuyPrice.text = (itemCount * selectItemData.price).ToString();
        }
    }

    private void OnEnable()
    {
        ResetUIData();
    }

    /// <summary>
    /// 아이템 카운트 중가 함수
    /// </summary>
    public void OnClickUpCountArrow()
    {
        if(selectItemData == null || string.IsNullOrEmpty(selectItemData.itemName))
        {
            return;
        }

        ItemCount = int.Parse(itemCountText.text);
        ItemCount++;
        itemCountText.text = ItemCount.ToString();
    }

    /// <summary>
    /// 아이템 카운트 감소 함수
    /// </summary>
    public void OnClickDownCountArrow()
    {
        if (selectItemData == null || string.IsNullOrEmpty(selectItemData.itemName))
        {
            return;
        }

        ItemCount = int.Parse(itemCountText.text);
        if (ItemCount <= 0)
            return;

        ItemCount--;
        itemCountText.text = ItemCount.ToString();
    }

    /// <summary>
    /// 아이템 정보 세팅해주는 함수
    /// </summary>
    /// <param name="itemData"></param>
    public void SetItemData(ItemData itemData)
    {
        selectItemData = itemData;
        ResetUIData();

        itemImage.sprite = SpriteManager.Instance.GetSprite(itemData.spriteName);
        itemImage.enabled = true;
        itemName.text = itemData.itemName;
        itemDesc.text = itemData.itemDesc;
    }

    public void SetItemCount(int count)
    {
        ItemCount = count;
        itemCountText.text = ItemCount.ToString();
    }


    /// <summary>
    /// 초기화 함수
    /// </summary>
    void ResetUIData()
    {
        itemImage.enabled = false;
        itemName.text = string.Empty;
        itemDesc.text = string.Empty;
        itemCountText.text = "0";
        totalBuyPrice.text = "0";
    }


    /// <summary>
    /// 직접 입력 버튼 클릭 시 팝업 표출 해주는 함수
    /// </summary>
    public void ShowAmountInputPopup()
    {
        if (string.IsNullOrEmpty(itemName.text))
            return;

        inputPopup.gameObject.SetActive(true);
    }
    public void BuyItemSetPopup()
    {
        if (selectItemData == null || string.IsNullOrEmpty(selectItemData.itemName) || ItemCount == 0)
        {
            return;
        }

        PopupManager.Instance.Init(string.Format("{0} 을 {1} 개 구매하시겠습니까?\n 가격은 {2}원 입니다.",
            itemName.text, itemCount, totalBuyPrice.text), ()=>
            {
                StartCoroutine(BuyItem());
            }, 
            true,
            () =>
            {
                PopupManager.Instance.PopupActive(false);
            });
    }

    public IEnumerator BuyItem()
    { 
        WWWForm form = new WWWForm();
        form.AddField("id", GameManager.Instance.PlayerData.ID);
        form.AddField("amount", int.Parse(totalBuyPrice.text));

        yield return StartCoroutine(DataManager.GameConnect("player/buyItem", form, data =>
        {

            JSONNode json = JSONNode.Parse(data);
            Debug.Log(json);

            if (json["success"].AsBool)
            {
                GameManager.Instance.PlayerInventoryData.AddItem(selectItemData, ItemCount);
                GameManager.Instance.PlayerData.Gold = json["gold"];
                PopupManager.Instance.Init("구매가 완료되었습니다.", () =>
                {
                    PopupManager.Instance.PopupActive(false);
                });
            }
            else
            {
                PopupManager.Instance.Init(json["message"], () => PopupManager.Instance.PopupActive(false));
            }
        }));





        //if(GameManager.Instance.PlayerData.GetMyGold() < int.Parse(totalBuyPrice.text))
        //{
        //    PopupManager.Instance.Init("보유한 골드가 부족합니다!", ()=> PopupManager.Instance.PopupActive(false));
        //}
        //else
        //{
        //    GameManager.Instance.PlayerInventoryData.AddItem(selectItemData, ItemCount);
        //    GameManager.Instance.PlayerData.Gold -= int.Parse(totalBuyPrice.text);
        //    PopupManager.Instance.Init("구매가 완료되었습니다.", ()=>
        //    {
        //        PopupManager.Instance.PopupActive(false);
        //    });
        //}
    }
}
