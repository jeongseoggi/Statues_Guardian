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
    /// ������ ī��Ʈ �߰� �Լ�
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
    /// ������ ī��Ʈ ���� �Լ�
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
    /// ������ ���� �������ִ� �Լ�
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
    /// �ʱ�ȭ �Լ�
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
    /// ���� �Է� ��ư Ŭ�� �� �˾� ǥ�� ���ִ� �Լ�
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

        NoticePopup noticePopup = PopupManager.Instance.noticePopup;

        noticePopup.Init(string.Format("{0} �� {1} �� �����Ͻðڽ��ϱ�?\n ������ {2}�� �Դϴ�.",
        itemName.text, itemCount, totalBuyPrice.text), ()=>
        {
            StartCoroutine(BuyItem());
        }, 
        true,
        () =>
        {
            noticePopup.Close();
        });
    }

    public IEnumerator BuyItem()
    { 
        WWWForm form = new WWWForm();
        form.AddField("id", GameManager.Instance.PlayerData.ID);
        form.AddField("amount", int.Parse(totalBuyPrice.text));

        NoticePopup noticePopup = PopupManager.Instance.noticePopup;

        yield return StartCoroutine(DataManager.GameConnect("player/buyItem", form, data =>
        {

            JSONNode json = JSONNode.Parse(data);
            Debug.Log(json);

            if (json["success"].AsBool)
            {
                GameManager.Instance.PlayerInventoryData.AddItem(selectItemData, ItemCount);
                GameManager.Instance.PlayerData.Gold = json["gold"];
                noticePopup.Init("���Ű� �Ϸ�Ǿ����ϴ�.", () =>
                {
                    noticePopup.Close();
                });
            }
            else
            {
                noticePopup.Init(json["message"], () => PopupManager.Instance.noticePopup.Close());
            }
        }));

    }
}
