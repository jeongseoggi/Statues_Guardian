using DG.Tweening.Core.Easing;
using UnityEngine;

public class DungeonManager : SingleTonDestory<DungeonManager>
{
    #region private
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] StageManager stageManager;
    [SerializeField] WaveManager waveManager;
    #endregion  

    #region 프로퍼티
    public SpawnManager SpawnManager { get => spawnManager; }
    public StageManager StageManager { get => stageManager; }
    public WaveManager WaveManager { get => waveManager; }
    #endregion



    private void Start()
    {
        UIManager.Instance.gameObject.SetActive(true);
        GameManager.Instance.curSceneName = "DungeonScene";
        GameManager.Instance.nextSceneName = string.Empty;
    }

}
