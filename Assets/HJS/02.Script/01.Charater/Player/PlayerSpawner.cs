using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    string spawnId;
    SpawnPoint spawnPoint;

    void Start()
    {
        SetSpawn();
        if (spawnPoint != null)
        {
            if(GameManager.Instance.PlayerStatData != null)
            {
                Spawn();
            }
            else
            {
                GameManager.OnPlayerStatDataReady += Spawn;
            }
        }
        else
        {
            Debug.LogWarning("SpawnPoint를 찾을 수 없습니다: " + spawnId);
        }
    }

    private void SetSpawn()
    {
        spawnId = GameManager.Instance.nextSpawnPointId;
        spawnPoint = SpawnPointManager.GetPoint(spawnId);
    }

    private void Spawn()
    {
        Player player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
        GameManager.Instance.Player = player;

        if(spawnId.Equals(GameString.VILLAGE_SPAWN_POINT))
        {
            SoundManager.Instance.PlayBGM(DataManager.Instance.GetAudioClip(GameString.VILLAGE_BGM_STRING));
        }
        else
        {
            SoundManager.Instance.PlayBGM(DataManager.Instance.GetAudioClip(GameString.DUNGEON_BGM_STRING));
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerStatDataReady -= Spawn;
    }
}
