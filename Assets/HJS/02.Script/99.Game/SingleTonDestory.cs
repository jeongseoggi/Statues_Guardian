using UnityEngine;

public class SingleTonDestory<T> : MonoBehaviour where T : SingleTonDestory<T>
{
    protected static T instance;

    public static T Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
    }
}
