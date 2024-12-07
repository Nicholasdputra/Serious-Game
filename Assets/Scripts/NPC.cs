using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Milo miloScript;
    public GameObject[] setDestination;
    public GameObject target;

    [Header ("Movement")]
    public bool canMove;
    public Pathfinding pathfinding;
    public float runawaySpeed = 2f;
    public float followSpeed = 1f;
    public float usingSpeed = 2f;
    private List<AStar_Node> pathToTarget;
    [SerializeField] int targetIndex;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize(){
        // canMove = true;
        setDestination = GameObject.FindGameObjectsWithTag("Set Destination");
        pathfinding = GameObject.FindWithTag("Pathfinding AI").GetComponent<Pathfinding>();
        miloScript = GameObject.FindWithTag("Milo").GetComponent<Milo>();
        rb = GetComponent<Rigidbody2D>();
        target = setDestination[Random.Range(0, setDestination.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove && !DestinationScript.instance.isGameOver){
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
        }else{
            pathToTarget = temp;
            targetIndex = 0;
            Debug.Log("Path recalculated: " + pathToTarget.Count + " nodes");
        }
        
        if (targetIndex < pathToTarget.Count)
        {
            Vector3 targetPosition = pathToTarget[targetIndex].worldPos;
            
            // Debug.Log("targetPosition: " + targetPosition);
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, usingSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                targetIndex++;
            }
        }
    }

    void OnDrawGizmos(){
        // if (pathToTarget == null)
        // {
        //     Debug.Log("No path to target.");
        //     return;
        // }
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