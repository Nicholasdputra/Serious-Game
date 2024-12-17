using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform NPC, destination;
    public AStar_Grid grid;

    void Awake()
    {
        grid = GetComponent<AStar_Grid>();
    }

    public List<AStar_Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Debug.Log("Finding path from " + startPos + " to " + targetPos);
        List<AStar_Node> path = new List<AStar_Node>();
        
        // Debug.Log("Finding path from " + startPos + " to " + targetPos);
        AStar_Node startNode = grid.GetNodePos(startPos);
        AStar_Node targetNode = grid.GetNodePos(targetPos);

        List <AStar_Node> openSet = new List<AStar_Node>();
        HashSet<AStar_Node> closedSet = new HashSet<AStar_Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            AStar_Node curr = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fcost() <= curr.fcost()) 
                // && openSet[i].hCost < curr.hCost)
                {
                    curr = openSet[i];
                }
            }
            openSet.Remove(curr);
            closedSet.Add(curr);

            if(curr == targetNode)
            {
                path.Add(targetNode);
                RetracePath(startNode, targetNode, path);
                Debug.Log("Path found, Length: " + path.Count);
                return path;
            }

            foreach (AStar_Node neighbour in grid.GetNeighbours(curr))
            {
                if(!neighbour.walkable 
                || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = curr.gCost + GetDistance(curr, neighbour);
                if(newCostToNeighbour < neighbour.gCost 
                || !openSet.Contains(neighbour))
                {
                    // Debug.Log("Cost: " + newCostToNeighbour);
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = curr;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        Debug.Log("Path not found");
        // RetracePath(startNode, targetNode, path);
        return new List<AStar_Node>();
    }

    List<AStar_Node> RetracePath(AStar_Node startNode, AStar_Node endNode, List<AStar_Node> path)
    {
        AStar_Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
        return path;
    }

    public int GetDistance(AStar_Node nodeA, AStar_Node nodeB){
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        } 
        else 
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}