using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap; // ���� Ÿ�ϸ�
    public Tilemap obstacleTilemap; // ��ֹ� Ÿ�ϸ�

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

                // Ÿ���� �����ϴ� ���� ó��
                if (!tilemap.HasTile(cell))
                    continue;

                Vector2Int pos = new Vector2Int(x, y);
                bool isWalkable = !obstacleTilemap.HasTile(cell);

                nodes[pos] = new Node(pos, isWalkable);
            }
        }

        Debug.Log($"������ ���� �� ��: {nodes.Count}��");
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
            new Vector2Int(0, 1),   // ��
            new Vector2Int(1, 0),   // ������
            new Vector2Int(0, -1),  // �Ʒ�
            new Vector2Int(-1, 0)   // ����
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
