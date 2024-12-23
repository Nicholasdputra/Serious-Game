using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardNPC : NPC
{
    protected float maxChaseRange;
    protected Vector3 startPos;
    protected Vector3 currPos;
    public GameObject anchor;
    public bool canTargetMilo;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("GuardNPC Start");
        Initialize();
        recalculatePath = null;
        canTargetMilo = true;
        maxChaseRange = 10f;
        usingSpeed = miloScript.defaultSpeed/2;
        canMove = true;
        startPos = transform.position;
        anchor = new GameObject("Anchor");
        anchor.transform.position = startPos;
        target = anchor;
        StartCoroutine(DelayedStart());
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!canUpdate){
            // Debug.Log("Can't Update"); 
            return;
        } 

        Debug.Log("Path Count:" + path.Count);
        // if((path == null || path.Count == 0) && target != anchor){
        //     Debug.Log("Path is null or path count is 0.");
        //     recalculatePath = null;
        //     recalculatePath = StartCoroutine(ReFindPath());
        // }

        if(!DestinationScript.isGameOver && canMove){
            // Debug.Log("Can Move and game isnt over yet - guard");
            CheckForMilo();
        }
        
        if(target == miloScript)
        {
            recalculateDelay = 0.2f;
            if(Vector3.Distance(transform.position, target.transform.position) > 10f){
                recalculateDelay = 0.6f;
            }
            if(recalculatePath == null)
            {
                recalculatePath = StartCoroutine(ReFindPath());
            }
        }
        else if(target == anchor)
        {
            recalculateDelay = 1f;
            if(recalculatePath == null)
            {
                recalculatePath = StartCoroutine(ReFindPath());
            }
        }
    }

    void CheckForMilo()
    {
        if (Vector3.Distance(startPos, miloScript.transform.position) < maxChaseRange && canTargetMilo)
        {
            // Debug.Log("Milo is within range");
            target = miloScript.gameObject;
        }
        else
        {
            // Debug.Log("Milo is not within range");
            target = anchor;
        }
        // Debug.Log("Following path");
        FollowPath();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log(this + "Collision detected with " + other.gameObject.name);
        if((other.gameObject.CompareTag("Milo") && target == miloScript.gameObject) || (other.gameObject.CompareTag("Milo") && canTargetMilo))
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            quickTime.qteNPC = this;
            if(!QuickTime.isQuickTimeActive){
                quickTime.StartQuickTimeEvent();
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // Debug.Log(this + "Collision detected with " + other.gameObject.name);
        if((other.gameObject.CompareTag("Milo") && target == miloScript.gameObject) || (other.gameObject.CompareTag("Milo") && canTargetMilo))
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            quickTime.qteNPC = this;
            if(quickTime.qteNPC == null){
                Debug.Log("qteNPC is null, called from ChasingNPC");
            }
            if(!QuickTime.isQuickTimeActive){
                quickTime.StartQuickTimeEvent();
                // Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPos, 10f);
    }
}
