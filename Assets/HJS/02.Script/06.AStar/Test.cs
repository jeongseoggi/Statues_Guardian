using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    List<Node> currentPath;
    int currentPathIndex;
    [SerializeField] GameObject targetPos;
    [SerializeField] Tilemap tilemap;
    [SerializeField] PathFinding pathfinder;
    float pathUpdateCooldown = 0.5f;
    float lastPathUpdateTime;
    public float speed = 3f;



    void Start()
    {
        FollowPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastPathUpdateTime > pathUpdateCooldown)
        {
            FollowPlayer();
            lastPathUpdateTime = Time.time;
        }

        MoveAlongPath();
    }

    void FollowPlayer()
    {
        Vector3Int cellPos3D = tilemap.WorldToCell(transform.position);
        Vector2Int monsterCellPos = new Vector2Int(cellPos3D.x, cellPos3D.y);

        Vector3Int cellPos3D2 = tilemap.WorldToCell(targetPos.transform.position);
        Vector2Int playerCellPos = new Vector2Int(cellPos3D2.x, cellPos3D2.y);

        currentPath = pathfinder.FindPath(monsterCellPos, playerCellPos);
        currentPathIndex = 0;
    }

    void MoveAlongPath()
    {
        if (currentPath == null || currentPathIndex >= currentPath.Count)
            return;

        Vector3 targetPos = tilemap.CellToWorld(new Vector3Int(
            currentPath[currentPathIndex].Position.x,
            currentPath[currentPathIndex].Position.y,
            0
            )) + tilemap.cellSize / 2f;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentPathIndex++;
        }
    }
}
