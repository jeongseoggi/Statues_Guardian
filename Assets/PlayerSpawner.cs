using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    void Start()
    {
        var spawnId = GameManager.Instance.nextSpawnPointId;
        var spawnPoint = SpawnPointManager.GetPoint(spawnId);

        if (spawnPoint != null)
        {
            Player player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
            GameManager.Instance.Player = player;
        }
        else
        {
            Debug.LogWarning("SpawnPoint를 찾을 수 없습니다: " + spawnId);
        }
    }
}
