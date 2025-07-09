using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "BuffData", menuName = "Buff/BuffData")]
public class BuffData : ScriptableObject
{
    public int buffID;
    public string buffName;
    public string buffDesc;
    public float duration;
    public float increase;
    public List<BuffType> buffEfeects;
    public string spriteName;

    public void BuffEffect(IBuffUsable user)
    {
        foreach (var buff in buffEfeects)
        {
            DataManager.Instance.BuffEffectDic[buff].UseEffect(user, duration, increase, buffName);
        }
        BuffNotifier.NotifyBuffAdded(SpriteManager.Instance.GetBuffSprite(spriteName), buffName, buffDesc);
        SoundManager.Instance.PlaySFX(DataManager.Instance.GetAudioClip("UseBuff"));
    }
}



