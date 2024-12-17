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
    protected List<AStar_Node> path;
    [SerializeField] int targetIndex;
    public Rigidbody2D rb;
    public float recalculateDelay;

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
        recalculateDelay = 1f;
        recalculateDelay = 1f;
        rb = GetComponent<Rigidbody2D>();
        if(waypointsToGoTo.Count != 0)
        {
            target = waypointsToGoTo[0].gameObject;
        }
        targetIndex = 0;
        path = new List<AStar_Node>();
        StartCoroutine(ReFindPath());
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove && !DestinationScript.instance.isGameOver)
        {
            FollowPath();
        }
    }

    public void FollowPath(){
        if (targetIndex < path.Count)
        {
            Vector3 targetPosition = path[targetIndex].worldPos;
            // Debug.Log("targetPosition: " + targetPosition);
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, usingSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetIndex++;
            }
        }

        if(waypointsToGoTo.Count != 0  && target == waypointsToGoTo[0] && Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            Debug.Log("Reached target " + target.name);
            //Dequeue waypointsToGoTo[0], add it back at the end
            GameObject placeholder = waypointsToGoTo[0];
            waypointsToGoTo.RemoveAt(0);
            waypointsToGoTo.Add(placeholder);
            target = waypointsToGoTo[0];
        }
    }

    public IEnumerator ReFindPath()
    {
        // if (pathfinding == null)
        // {
        //     Debug.Log("Pathfinding is null.");
        //     return;
        // }
        while(true){
            path = pathfinding.FindPath(transform.position, target.transform.position);
            // Debug.Log("FollowPath");
            if (path == null || path.Count == 0)
            {
                Debug.Log("No path to target.");
                // Find the nearest walkable node to the target
                AStar_Node targetNode = pathfinding.grid.GetNodePos(target.transform.position);
                AStar_Node nearestWalkableNode = FindNearestWalkableNode(targetNode);

                if (nearestWalkableNode != null)
                {
                    Debug.Log("Moving to nearest walkable node.");
                    path = pathfinding.FindPath(transform.position, nearestWalkableNode.worldPos);
                }
                // pathToTarget = temp;
                // targetIndex = 0;
            }
            yield return new WaitForSeconds(recalculateDelay);
        }
        // else
        // {
            // pathToTarget = temp;
            // targetIndex = 0;
            // Debug.Log("Path recalculated: " + pathToTarget.Count + " nodes");
        // }
        
        
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
        if (path == null)
        {
            // Debug.Log("No path to target.");
            return;
        }
        // if(targetIndex == 0){
        //     Debug.Log("Target index is 0.");
        //     return;
        // }
        for (int i = targetIndex; i < path.Count; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(path[i].worldPos, Vector3.one * (pathfinding.grid.nodeDiameter - 0.1f));
        }
    }
}