using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform NPC, destination;
    AStar_Grid grid;

    void Awake(){
        grid = GetComponent<AStar_Grid>();
    }

    void findPath(Vector3 startPos, Vector3 targetPos){
        Stopwatch sw = new Stopwatch();
        sw.Start();
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
                if(openSet[i].fcost() < curr.fcost() || openSet[i].fcost() == curr.fcost() && openSet[i].hCost < curr.hCost){
                    curr = openSet[i];
                }
            }
            openSet.Remove(curr);
            closedSet.Add(curr);

            if(curr == targetNode){
                // Debug.Log("Path found");
                sw.Stop();
                // UnityEngine.Debug.Log("Path found in: " + sw.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (AStar_Node neighbour in grid.GetNeighbours(curr)){
                if(!neighbour.walkable || closedSet.Contains(neighbour)){
                    continue;
                }

                int newCostToNeighbour = curr.gCost + GetDistance(curr, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = curr;

                    if(!openSet.Contains(neighbour)){
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(AStar_Node startNode, AStar_Node endNode){
        List<AStar_Node> path = new List<AStar_Node>();
        AStar_Node currentNode = endNode;

        while(currentNode != startNode){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }

    int GetDistance(AStar_Node nodeA, AStar_Node nodeB){
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY){
            return 14 * distY + 10 * (distX - distY);
        } else {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetButtonDown("Jump")){
            findPath(NPC.position, destination.position);
        // }
    }
}
