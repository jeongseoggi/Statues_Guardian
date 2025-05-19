using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap; // 기준 타일맵
    public Tilemap obstacleTilemap; // 장애물 타일맵

    private Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();

    void Awake()
    {
        GenerateGridFromUsedTiles();
    }

    void GenerateGridFromUsedTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);

                // 타일이 존재하는 셀만 처리
                if (!tilemap.HasTile(cell))
                    continue;

                Vector2Int pos = new Vector2Int(x, y);
                bool isWalkable = !obstacleTilemap.HasTile(cell);

                nodes[pos] = new Node(pos, isWalkable);
            }
        }

        Debug.Log($"실제로 사용된 셀 수: {nodes.Count}개");
    }

    public Node GetNode(Vector2Int pos)
    {
        nodes.TryGetValue(pos, out Node node);
        return node;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // 위
            new Vector2Int(1, 0),   // 오른쪽
            new Vector2Int(0, -1),  // 아래
            new Vector2Int(-1, 0)   // 왼쪽
        };

        foreach (var dir in directions)
        {
            Vector2Int neighborPos = node.Position + dir;

            if (nodes.TryGetValue(neighborPos, out Node neighbor) && neighbor.IsWalkable)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
