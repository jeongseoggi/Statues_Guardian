using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public int totalWave;
    public int[] monstersPerWave;
    public int[] spawnMonsterType;
}
