using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] protected Milo miloScript;
    // public GameObject[] setDestination;
    public GameObject target;
    public List<GameObject> waypointsToGoTo;
    
    [Header ("Movement")]
    public bool canMove;
    [SerializeField] protected Pathfinding pathfinding;
    [SerializeField] protected float usingSpeed = 2f;
    protected List<AStar_Node> path;
    [SerializeField] int targetIndex;
    public Rigidbody2D rb;
    public float recalculateDelay;
    protected bool canUpdate;
    protected Coroutine recalculatePath;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        canMove = true;
        target = waypointsToGoTo[0];
    }

    protected IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        canUpdate = true; // Allow Update logic to start
    }

    public void Initialize()
    {
        // canMove = true;
        canUpdate = false;
        pathfinding = GameObject.FindWithTag("Pathfinding AI").GetComponent<Pathfinding>();
        miloScript = GameObject.FindWithTag("Milo").GetComponent<Milo>();
        recalculateDelay = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        if(waypointsToGoTo.Count != 0)
        {
            target = waypointsToGoTo[0].gameObject;
        }
        targetIndex = 0;
        path = new List<AStar_Node>();

        // Debug.Log("For " + gameObject.name + ":");
        // Debug.Log("Pathfinding initialized: " + (pathfinding != null));
        // Debug.Log("MiloScript initialized: " + (miloScript != null));
        // Debug.Log("Rigidbody2D initialized: " + (rb != null));
        // Debug.Log("Target initialized: " + (target != null));
        StartCoroutine(DelayedStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (!canUpdate) return;
        if(recalculatePath == null){
            recalculatePath = StartCoroutine(ReFindPath());
        }
        if(target == miloScript && Vector3.Distance(transform.position, target.transform.position) > 10f)
        {
            recalculateDelay = 0.6f;
        }
        else if(target == miloScript)
        {
            recalculateDelay = 0.2f;
        }

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

        // if(waypointsToGoTo.Count != 0  && target == waypointsToGoTo[0]){
        //     Debug.Log("Reached target " + target.name);
        // }
        // Debug.Log("Distance to target: " + Vector3.Distance(transform.position, target.transform.position));
        if(waypointsToGoTo.Count != 0  && target == waypointsToGoTo[0] && Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            // Debug.Log("Reached target " + target.name);
            //Dequeue waypointsToGoTo[0], add it back at the end
            GameObject placeholder = waypointsToGoTo[0];
            waypointsToGoTo.RemoveAt(0);
            waypointsToGoTo.Add(placeholder);
            target = waypointsToGoTo[0];
        }
    }

    public IEnumerator ReFindPath()
    {
        // Debug.Log("Calling ReFindPath");
        // if (pathfinding == null)
        // {
        //     Debug.Log("Pathfinding is null.");
        //     return;
        // }
        while(true){
            targetIndex = 0;
            // Debug.Log("Grid is null: " + pathfinding.grid == null);
            AStar_Node targetNode = pathfinding.grid.GetNodePos(target.transform.position);
            AStar_Node nearestWalkableNode = FindNearestWalkableNode(targetNode);
            if(targetNode.walkable){
                nearestWalkableNode = targetNode;
            }
            path = pathfinding.FindPath(transform.position, nearestWalkableNode.worldPos);
            // Debug.Log("FollowPath");
            if (path == null || path.Count == 0)
            {
                Debug.Log("No path to target.");
                // Find the nearest walkable node to the target

                if (nearestWalkableNode != null)
                {
                    Debug.Log("Moving to nearest walkable node.");
                    path = pathfinding.FindPath(transform.position, nearestWalkableNode.worldPos);
                }
                // pathToTarget = temp;
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