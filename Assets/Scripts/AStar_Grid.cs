using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_Grid : MonoBehaviour
{
    public bool onlyDisplayPathGizmos;
    // public Transform NPC;
    public LayerMask unwalkable; 
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public AStar_Node[,] grid;
    
    public float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start(){
        transform.position = new Vector3(0, 0, 0);
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void Update(){
        // StartCoroutine(UpdateGrid());
    }

    public int MaxSize{
        get{
            return gridSizeX * gridSizeY;
        }
    }

    public void CreateGrid(){
        // Debug.Log("Gridsize x: " + gridSizeX + " Gridsize y: " + gridSizeY);
        grid = new AStar_Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
                                                                    //left edge of grid                  //bottom edge of grid
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                //Finding the world position of the node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                
                //Checking if the node is walkable or not
                bool walkable;
                if(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkable)){
                    walkable = false;
                } else {
                    walkable = true;
                }

                //Marking the node as walkable or not
                grid[x,y] = new AStar_Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<AStar_Node> GetNeighbours(AStar_Node node){
        List<AStar_Node> neighbours = new List<AStar_Node>();

        if(CheckNeighbour(node.gridX+1, node.gridY)){
            neighbours.Add(grid[node.gridX+1, node.gridY]);
        }

        if(CheckNeighbour(node.gridX-1, node.gridY)){
            neighbours.Add(grid[node.gridX-1, node.gridY]);
        }

        if(CheckNeighbour(node.gridX, node.gridY+1)){
            neighbours.Add(grid[node.gridX, node.gridY+1]);
        }
        
        if(CheckNeighbour(node.gridX, node.gridY-1)){
            neighbours.Add(grid[node.gridX, node.gridY-1]);
        }

        return neighbours;
    }

    public bool CheckNeighbour(int x, int y)
    {
        if(x >= 0 && x < gridSizeX){
            if(y >= 0 && y < gridSizeY){
                return true;
            }
        }
        return false;
    }

    public List<AStar_Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));
        if(onlyDisplayPathGizmos){
            // if(path != null){
            //     foreach (AStar_Node node in path){
            //         Gizmos.color = Color.black;
            //         Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            //     }
            // }
            return;
        } else{
            if(grid != null){
                // AStar_Node npcNode = GetNodePos(NPC.position);
                //Colouring the walkable and unwalkable nodes with white and red respectively
                Debug.Log(grid.Length);
                foreach (AStar_Node node in grid){
                    if(node.walkable){
                        Gizmos.color = Color.white;
                    } else {
                        Gizmos.color = Color.red;
                    }   
                
                    // if(path != null){
                    //     if(path.Contains(node)){
                    //         Gizmos.color = Color.black;
                    //     }
                    // }
                    Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
    }
    
    public AStar_Node GetNodePos(Vector3 worldPosition){
        // Converting world position to percentage
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        // Clamp the percentages to ensure they are within the valid range
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Getting the x and y values of the grid
        int x = Mathf.FloorToInt((gridSizeX) * percentX);
        int y = Mathf.FloorToInt((gridSizeY) * percentY);
        
        return grid[x,y];
    }
}