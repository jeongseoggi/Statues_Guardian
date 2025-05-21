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
    /// ������ DB���� ������ �����͸� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public ItemData GetItemData(string itemName)
    {
        return ItemDataBase.itemData.Find((x) => x.itemName.Equals(itemName));
    }

    /// <summary>
    /// ������ Ÿ�Կ� ���� ���� ����
    /// </summary>
    public void ItemInit()
    {
        //Ÿ�Կ� �´� ������ ������ �־���
        foreach (var itemData in ItemDataBase.itemData)
        {
            switch(itemData.itemType)
            {
                case ItemType.Heal:
                    itemData.itemUseStrategy = new HealItemStrategy();
                    break;
                case ItemType.Upgrade:
                    itemData.itemUseStrategy = new UpgradeItemStrategy();
                    break;
            }

        }

    }


    /// <summary>
    /// �ڷ�ƾ ����� �Ұ����� Ŭ�������� ��� �����ϵ��� ���� �Լ�
    /// </summary>
    /// <param name="apiName"></param>
    /// <param name="form"></param>
    /// <param name="successAction"></param>
    public void GameConnectHelper(string apiName, WWWForm form, UnityAction<string> successAction)
    {
        StartCoroutine(GameConnect(apiName, form, successAction));
    }
    
    /// <summary>
    /// ������ �����͸� �����ϰų� ���� �� �ֵ��� ���ִ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="apiName">api��</param>
    /// <param name="json">������ ���� ������</param>
    /// <param name="successAction">���� ���� Action</param>
    /// <returns></returns>
    public static IEnumerator GameConnect(string apiName, WWWForm json, UnityAction<string> successAction)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(ServerURL + apiName, json))
        {
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Server Data => {request.downloadHandler.text}");
                successAction?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"[GameConnect] ���� ��� ����: {request.error}");
            }
        }
    }

    public IEnumerator ItemUse(string apiName, WWWForm form, UnityAction<string> successAction)
    {
        yield return StartCoroutine(GameConnect(apiName, form, successAction));
    }
}
