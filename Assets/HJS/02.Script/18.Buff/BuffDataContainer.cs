using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffDataContainer", menuName = "Buff/BuffDataContainer")]
public class BuffDataContainer : ScriptableObject
{
    public List<BuffData> buffDataList;

    public void Initalize()
    {
        buffDataList = new List<BuffData>();
    }
}
