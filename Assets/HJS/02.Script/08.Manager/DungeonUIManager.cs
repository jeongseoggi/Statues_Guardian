using UnityEngine;
using UnityEngine.UI;

public class DungeonUIManager : SingleTonDestory<DungeonUIManager>
{
    public WaveText waveText;
    public Button nextWaveStartBTN;

    public WaveText WaveText
    {
        get
        {
            return waveText;
        }
    }
}
