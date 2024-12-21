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
    public float lowDelay;
    public float highDelay;

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
        highDelay = Random.Range(0.5f, 0.8f);
        lowDelay = Random.Range(0.2f, 0.5f);
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
            recalculateDelay = highDelay;
        }
        else if(target == miloScript)
        {
            recalculateDelay = lowDelay;
        }

        if(canMove && !DestinationScript.isGameOver)
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
            // Debug.Log("CurrNode");
            // Debug.Log("TargetNode");
            AStar_Node currNode = pathfinding.grid.GetNodePos(transform.position);
            AStar_Node targetNode = pathfinding.grid.GetNodePos(target.transform.position);
            AStar_Node nearestWalkableNode = FindNearestWalkableNode(targetNode,0);
            // Debug.Log("curr Node" + currNode.walkable);
            // Debug.Log("Destination Node" + nearestWalkableNode.walkable);

            path = pathfinding.FindPath(transform.position, nearestWalkableNode.worldPos, gameObject.name);
            // Debug.Log("FollowPath");
            if (path == null || path.Count == 0)
            {
                Debug.Log("No path to target.");
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

    AStar_Node FindNearestWalkableNode(AStar_Node targetNode, int counter)
    {
        if(targetNode.walkable){
            // Debug.Log("Target Node is walkable.");
            return targetNode;
        }
        return IterateThroughNeighbours(targetNode, 0);
    }

    AStar_Node IterateThroughNeighbours(AStar_Node targetNode, int counter)
    {
        if(counter > 3){
            return null;
        }
        List<AStar_Node> neighbours = pathfinding.grid.GetNeighbours(targetNode);
        foreach (AStar_Node neighbor in neighbours)
        {
            if (neighbor.walkable)
            {
                // Debug.Log("Found walkable node.");
                Debug.DrawLine(neighbor.worldPos, neighbor.worldPos+Vector3.up);
                return neighbor;
            }
        }
        foreach (AStar_Node neighbour in neighbours)
        {
            AStar_Node walkableNode = IterateThroughNeighbours(neighbour, counter+1);
            if(walkableNode != null){
                return walkableNode;
            }
        }
        return null;
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