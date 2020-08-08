using PlatformerPathFinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    // This script is loosely based on a video by CodeMonkey but adapted to use the PathfindingGrid we already have from the raptor AI
    // Summary of how A* pathfinding works
    // The goal is to recieve a start point and end point and return the shortest path to the end and avoiding unwalkable areas
    // The algorith will start at the position called start and will check if already at end
    // if not will loop through all neioghbouring nodes that are with in the grid and are not occupied and evaluate which neighbour has the lowest F cost
    // The f cost is the sum of the G cost (distance travveled to that node so far) + H cost (distance directly to end(this does not include blockages))
    // If a neighbour has a lower current F cost then the current node then that neighbour will have its neighbours evaluted
    // As the evaluation moves from one node to another it will keep track of the node it was at previous to use this to back track from the end to create the path that will be returned
    // If a node has been checked before it will be added to a closed list to ensure that it doesnt keeping going over the same nodes over and over

    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;
    private PathFindingGrid grid;
    private List<Node> openList;
    private List<Node> closedList;

    private void Start()
    {
        grid = GetComponent<PathFindingGrid>();
    }

    public List<Node> FindPath(Vector2 start, Vector2 end)
    {
        Node startNode = grid.WorldPositionToNode(start);
        Node endNode = grid.WorldPositionToNode(end);

        openList = new List<Node>();
        closedList = new List<Node>();

        openList.Add(startNode);

        for (int x = 0; x < grid._gridSizeX; x++)
        {
            for (int y = 0; y < grid._gridSizeY; y++)
            {
                Node node = grid.GetNode(x,y);
                node.GCost = int.MaxValue;
                node.cameFromNode = null;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            print(currentNode.worldPosition);

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbour);
                if (tentativeGCost < neighbour.GCost)
                {
                    neighbour.cameFromNode = currentNode;
                    neighbour.GCost = tentativeGCost;
                    neighbour.HCost = CalculateDistanceCost(neighbour, endNode);

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> GetNeighbours (Node currentNode)
    {
        List<Node> neighbourList = new List<Node>();
        if (grid.IsValidNeighbour(currentNode.x - 1, currentNode.y, out Node leftNeighbour))
        {
            neighbourList.Add(leftNeighbour);
        }
        if (grid.IsValidNeighbour(currentNode.x, currentNode.y + 1, out Node upNeighbour))
        {
            neighbourList.Add(upNeighbour);
        }
        if (grid.IsValidNeighbour(currentNode.x + 1, currentNode.y, out Node rightNeighbour))
        {
            neighbourList.Add(rightNeighbour);
        }
        if (grid.IsValidNeighbour(currentNode.x, currentNode.y - 1, out Node downNeighbour))
        {
            neighbourList.Add(downNeighbour);
        }
        if (grid.IsValidNeighbour(currentNode.x - 1, currentNode.y - 1, out Node downLeftNeighbour))
        {
            neighbourList.Add(downLeftNeighbour);
        }
        if (grid.IsValidNeighbour(currentNode.x - 1, currentNode.y + 1, out Node upLeftNeighbour))
        {
            neighbourList.Add(upLeftNeighbour);
        }
        if (grid.IsValidNeighbour(currentNode.x + 1, currentNode.y + 1, out Node upRightNeighbour))
        {
            neighbourList.Add(upRightNeighbour);
        }
        if (grid.IsValidNeighbour(currentNode.x + 1, currentNode.y - 1, out Node downRightNeighbour))
        {
            neighbourList.Add(downRightNeighbour);
        }
        
        return neighbourList;
    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    private Node GetLowestFCostNode (List<Node> nodeList)
    {
        Node lowestCostNode = nodeList[0];
        for (int i = 1; i < nodeList.Count; i++)
        {
            if ((nodeList[i].GCost + nodeList[i].HCost) < (lowestCostNode.GCost + lowestCostNode.HCost))
            {
                lowestCostNode = nodeList[i];
            }
        }
        return lowestCostNode;
    }
}
