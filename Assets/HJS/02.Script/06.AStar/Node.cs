using UnityEngine;

public class Node
{
    public Vector2Int Position;
    public Node Parent;
    public int GCost;
    public int HCost;
    public int FCost=> GCost + HCost;

    public bool IsWalkable;

    public Node(Vector2Int pos, bool isWalkable)
    {
        Position = pos;
        IsWalkable = isWalkable;
    }
}
