using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : SingleTon<T>
{
    protected static T instance;

    public static T Instance { get =>  instance; }

    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
