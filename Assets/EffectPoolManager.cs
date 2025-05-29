using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoolManager : SingleTon<EffectPoolManager>
{
    public Dictionary<EffectType, Queue<GameObject>> effectDic = new Dictionary<EffectType, Queue<GameObject>>();
    public List<GameObject> effectPrefabDic;
    public Dictionary<EffectType, int> effectPoolCount = new Dictionary<EffectType, int>()
    {
        {EffectType.HealEffect, 5},
        {EffectType.DotEffect, 15 },
        {EffectType.AoeEffect, 15 }
    };

    private void Start()
    {
        Init();
    }


    public void Init()
    {
        int index = 0;
        foreach (var kvp in effectPoolCount)
        {
            EffectType key = kvp.Key;
            int count = kvp.Value;
            Queue<GameObject> addQueue = new Queue<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject effectObj = Instantiate(effectPrefabDic[(int)key], transform);
                effectObj.SetActive(false);
                addQueue.Enqueue(effectObj);
            }
            effectDic.Add(key, addQueue);
            index++;
        }
    }


    public GameObject GetEffect(EffectType key, Transform parent = null)
    {
        if (effectDic.ContainsKey(key) && effectDic[key].Count > 0)
        {
            GameObject obj = effectDic[key].Dequeue();
            obj.SetActive(true);
            if(parent != null)
            {
                obj.transform.SetParent(parent, false);
            }
            return obj;
        }
        else
        {
            GameObject effectObj = Instantiate(effectPrefabDic[(int)key]);
            return effectObj;
        }
    }

    public void ReturnEffect(EffectType key, GameObject effectObj)
    {
        effectObj.SetActive(false);
        effectObj.transform.SetParent(gameObject.transform, false);
        effectDic[key].Enqueue(effectObj);
    }
}
