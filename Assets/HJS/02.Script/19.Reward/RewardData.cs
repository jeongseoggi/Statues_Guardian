using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "Reward/RewardData")]
public class RewardData : ScriptableObject
{
    public int rewardID;
    public int stageID;
    public string rewardDataName;
    public List<int> itemIDs;
    public List<int> amounts;
    public int gold;

    public void Init(int rewardID, int stageID, string rewardDataName, int gold)
    {
        this.rewardID = rewardID;
        this.stageID = stageID;
        this.rewardDataName = rewardDataName;
        this.gold = gold;
    }
}
 