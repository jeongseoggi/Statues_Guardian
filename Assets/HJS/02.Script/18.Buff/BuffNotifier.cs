using System.Collections.Generic;
using UnityEngine;

public static class BuffNotifier
{
    private static List<IBuffObserver> observers = new List<IBuffObserver>();

    public static void Subscribe(IBuffObserver observer)
    {
        if(!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public static void Unsubscribe(IBuffObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    /// <summary>
    /// 버프 활성 알림
    /// </summary>
    /// <param name="img"></param>
    /// <param name="buffName"></param>
    /// <param name="buffDesc"></param>
    public static void NotifyBuffAdded(Sprite img, string buffName, string buffDesc)
    {
        foreach (IBuffObserver observer in observers)
        {
            observer.OnBuffAdded(img, buffName, buffDesc);
        }
    }

    /// <summary>
    /// 버프 종료 알림
    /// </summary>
    /// <param name="buffName"></param>
    public static void NotifyBuffRemoved(string buffName)
    {
        foreach (var observer in observers)
        {
            observer.OnBuffRemoved(buffName);
        }
    }
}
