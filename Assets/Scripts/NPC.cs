using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Milo milo;
    public GameObject target;
    [Header ("Movement")]
    public bool canMove;
    public Pathfinding pathfinding;
    public float runawaySpeed = 2f;
    public float followSpeed = 1f;
    public float usingSpeed = 2f;
    private List<AStar_Node> pathToTarget;
    private int targetIndex;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize(){
        milo = GameObject.FindWithTag("Milo").GetComponent<Milo>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            FollowPath();
        }
    }

    public void FollowPath()
    {
        List<AStar_Node> temp = pathfinding.FindPath(transform.position, milo.transform.position);
        // Debug.Log("FollowPath");
        if (temp == null || temp.Count == 0)
        {
            // Debug.Log("No path to target.");
        }else{
            pathToTarget = temp;
            targetIndex = 0;
            // Debug.Log("Path recalculated: " + pathToTarget.Count + " nodes");
        }
        
        if (targetIndex < pathToTarget.Count)
        {
            Vector3 targetPosition = pathToTarget[targetIndex].worldPos;
            // Debug.Log("targetPosition: " + targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, usingSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                targetIndex++;
            }
        }
    }

    void OnDrawGizmos(){
        for (int i = targetIndex; i < pathToTarget.Count; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(pathToTarget[i].worldPos, Vector3.one * (pathfinding.grid.nodeDiameter - 0.1f));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Milo"))
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            if(!quickTime.isQuickTimeActive){
                quickTime.StartQuickTimeEvent();
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Milo"))
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            if(!quickTime.isQuickTimeActive){
                quickTime.StartQuickTimeEvent();
                Destroy(gameObject);
            }
        }
    }
}