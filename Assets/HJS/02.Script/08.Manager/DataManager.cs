using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DataManager : SingleTon<DataManager>
{
    private static string ServerURL = "http://localhost:3000/";

    [SerializeField] ItemScriptableObject itemDataBase;

    public ItemScriptableObject ItemDataBase { get => itemDataBase; }

    private void Start()
    {
        ItemInit();
    }
    /// <summary>
    /// 아이템 DB에서 아이템 데이터를 반환합니다.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public ItemData GetItemData(string itemName)
    {
        ItemData data = ItemDataBase.itemData.Find((x) => x.itemName.Equals(itemName));
        return data;
    }

    /// <summary>
    /// 아이템 타입에 따른 전략 저장
    /// </summary>
    public void ItemInit()
    {
        //타입에 맞는 아이템 전략을 넣어줌
        foreach (ItemData itemData in ItemDataBase.itemData)
        {
            switch(itemData.itemType)
            {
                case ItemType.Heal:
                    itemData.itemUseStrategy = new HealItemStrategy();
                    break;
            }

        }

    }
    
    /// <summary>
    /// 서버에 데이터를 전송하거나 받을 수 있도록 해주는 코루틴 함수
    /// </summary>
    /// <param name="apiName">api명</param>
    /// <param name="json">서버에 보낼 데이터</param>
    /// <param name="successAction">성공 후의 Action</param>
    /// <returns></returns>
    public static IEnumerator GameConnect(string apiName, WWWForm json, UnityAction<string> successAction)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(ServerURL + apiName, json))
        {
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                successAction?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"[GameConnect] 서버 통신 실패: {request.error}");
            }
        }
    }
}
