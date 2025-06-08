using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public BuffWindow buffPrefab;
    public List<BuffWindow> activeBuffList;

    private void Start()
    {
        activeBuffList = new List<BuffWindow>();
    }

    public void SetBuff(Sprite img, string buffName, string buffDesc)
    {
        BuffWindow buffWindow = Instantiate(buffPrefab, gameObject.transform);
        buffWindow.Init(img, buffName, buffDesc);
        activeBuffList.Add(buffWindow);
    }

    public void UnActiveBuff(string buffName)
    {
        BuffWindow unActiveBuff = activeBuffList.Find((x) => x.buffName.Equals(buffName));

        if(unActiveBuff != null)
        {
            unActiveBuff.gameObject.SetActive(false);
            activeBuffList.Remove(unActiveBuff);
        }
    }
}


