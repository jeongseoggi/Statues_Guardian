using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public GridManager gridManager; // 타일맵 정보를 제공해줄 클래스 (장애물 여부 등)

    public List<Node> FindPath(Vector2Int startPos, Vector2Int targetPos)
    {
        Node startNode = gridManager.GetNode(startPos);
        Node targetNode = gridManager.GetNode(targetPos);

        if (startNode == null || targetNode == null || !targetNode.IsWalkable)
            return null;

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        startNode.GCost = 0;
        startNode.HCost = GetManhattanDistance(startPos, targetPos);

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable || closedList.Contains(neighbor))
                    continue;

                int tentativeGCost = currentNode.GCost + GetManhattanDistance(currentNode.Position, neighbor.Position);
                if (tentativeGCost < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = GetManhattanDistance(neighbor.Position, targetNode.Position);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null; // 경로를 찾을 수 없음
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private int GetManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private Node GetLowestFCostNode(List<Node> nodeList)
    {
        Node lowestFCostNode = nodeList[0];
        foreach (Node node in nodeList)
        {
            if (node.FCost < lowestFCostNode.FCost ||
                (node.FCost == lowestFCostNode.FCost && node.HCost < lowestFCostNode.HCost))
            {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }
}
