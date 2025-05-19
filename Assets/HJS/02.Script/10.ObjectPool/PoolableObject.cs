using System.Collections.Generic;
using UnityEngine;

public class PoolableObject<T> : MonoBehaviour, IObjectPool<T> where T : Component
{
    public Queue<T> poolQueue = new Queue<T>();
    public int poolsize;
    public T poolObject;

    void Start()
    {
        Init(poolsize);
    }

    /// <summary>
    /// size��ŭ Queue�ȿ� �־���
    /// </summary>
    /// <param name="size"></param>
    public virtual void Init(int size)
    {
        for (int i = 0; i < poolsize; i++)
        {
            T go = Instantiate(poolObject, gameObject.transform);
            go.gameObject.SetActive(false);
            poolQueue.Enqueue(go);
        }
    }

    /// <summary>
    /// Ǯ���� ����
    /// </summary>
    /// <returns></returns>
    public virtual T SpawnPool()
    {
        if(poolQueue.Count == 0)
        {
            Init(poolsize / 2);
        }
        return poolQueue.Dequeue();
    }

    /// <summary>
    /// �ٽ� Ǯ�� ����
    /// </summary>
    /// <param name="poolObject"></param>
    public virtual void ReturnPool(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
        poolQueue.Enqueue(poolObject);
    }

}
