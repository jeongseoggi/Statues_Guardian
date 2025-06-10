using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour, IBuffObserver
{
    public BuffWindow buffPrefab;
    public List<BuffWindow> activeBuffWindowList;
    public Action buffSetAction;

    private void Start()
    {
        activeBuffWindowList = new List<BuffWindow>();
        BuffNotifier.Subscribe(this);
    }

    /// <summary>
    /// 버프 사용 구현부
    /// </summary>
    /// <param name="img"></param>
    /// <param name="buffName"></param>
    /// <param name="buffDesc"></param>
    public void OnBuffAdded(Sprite img, string buffName, string buffDesc)
    {
        BuffWindow buffWindow = Instantiate(buffPrefab, gameObject.transform);
        buffWindow.Init(img, buffName, buffDesc);
        activeBuffWindowList.Add(buffWindow);
    }

    /// <summary>
    /// 버프 제거 구현부
    /// </summary>
    /// <param name="buffName"></param>
    public void OnBuffRemoved(string buffName)
    {
        BuffWindow unActiveBuff = activeBuffWindowList.Find((x) => x.buffName.Equals(buffName));

        if (unActiveBuff != null)
        {
            unActiveBuff.gameObject.SetActive(false);
            activeBuffWindowList.Remove(unActiveBuff);
        }
    }

    public void OnDestroy()
    {
        BuffNotifier.Unsubscribe(this);
    }

}


