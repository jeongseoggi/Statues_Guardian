using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardContainer", menuName = "Reward/RewardContainer")]
public class RewardContainer : ScriptableObject
{
    public List<RewardData> rewardList;

    public void Initalize()
    {
        rewardList = new List<RewardData>();
    }
}
