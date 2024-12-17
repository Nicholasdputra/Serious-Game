using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    protected Milo miloScript;
    // public GameObject[] setDestination;
    public GameObject target;
    public List <GameObject> waypointsToGoTo;
    
    [Header ("Movement")]
    public bool canMove;
    protected Pathfinding pathfinding;
    protected float usingSpeed = 2f;
    protected List<AStar_Node> pathToTarget;
    [SerializeField] int targetIndex;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // canMove = true;
        pathfinding = GameObject.FindWithTag("Pathfinding AI").GetComponent<Pathfinding>();
        miloScript = GameObject.FindWithTag("Milo").GetComponent<Milo>();

        rb = GetComponent<Rigidbody2D>();
        if(waypointsToGoTo.Count != 0)
        {
            target = waypointsToGoTo[0].gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove && !DestinationScript.instance.isGameOver)
        {
            FollowPath();
        }
    }

    public void FollowPath()
    {
        // if (pathfinding == null)
        // {
        //     Debug.Log("Pathfinding is null.");
        //     return;
        // }
        
        List<AStar_Node> temp = pathfinding.FindPath(transform.position, target.transform.position);
        // Debug.Log("FollowPath");
        if (temp == null || temp.Count == 0)
        {
            Debug.Log("No path to target.");
            // Find the nearest walkable node to the target
            AStar_Node targetNode = pathfinding.grid.GetNodePos(target.transform.position);
            AStar_Node nearestWalkableNode = FindNearestWalkableNode(targetNode);

            if (nearestWalkableNode != null)
            {
                Debug.Log("Moving to nearest walkable node.");
                temp = pathfinding.FindPath(transform.position, nearestWalkableNode.worldPos);
            }
            // pathToTarget = temp;
            // targetIndex = 0;
        }
        // else
        // {
            pathToTarget = temp;
            targetIndex = 0;
            // Debug.Log("Path recalculated: " + pathToTarget.Count + " nodes");
        // }
        
        if (targetIndex < pathToTarget.Count)
        {
            Vector3 targetPosition = pathToTarget[targetIndex].worldPos;
            // Debug.Log("targetPosition: " + targetPosition);
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, usingSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
            }
        }

        if(waypointsToGoTo.Count != 0  && Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            Debug.Log("Reached target " + target.name);
            //Dequeue waypointsToGoTo[0], add it back at the end
            GameObject placeholder = waypointsToGoTo[0];
            waypointsToGoTo.RemoveAt(0);
            waypointsToGoTo.Add(placeholder);
            target = waypointsToGoTo[0];
        }
    }

    AStar_Node FindNearestWalkableNode(AStar_Node targetNode)
    {
        AStar_Node nearestNode = null;
        int minDistance = int.MaxValue;

        foreach (AStar_Node node1 in pathfinding.grid.GetNeighbours(targetNode))
        {
            foreach (AStar_Node node2 in pathfinding.grid.GetNeighbours(targetNode))
            {
                if (node2.walkable)
                {
                    int distance = pathfinding.GetDistance(node2, targetNode);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestNode = node2;
                    }
                }
            }
        }
        return nearestNode;
    }

    void OnDrawGizmos()
    {
        if (pathToTarget == null)
        {
            // Debug.Log("No path to target.");
            return;
        }
        // if(targetIndex == 0){
        //     Debug.Log("Target index is 0.");
        //     return;
        // }
        for (int i = targetIndex; i < pathToTarget.Count; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(pathToTarget[i].worldPos, Vector3.one * (pathfinding.grid.nodeDiameter - 0.1f));
        }
    }
}