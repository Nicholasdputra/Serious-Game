using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveToSneakNPC : GuardNPC
{
    private float detectRange;
    public bool onCooldown;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        canUpdate = false;
        maxChaseRange = 20f;
        usingSpeed = miloScript.defaultSpeed/2;
        canMove = true;
        startPos = transform.position;
        Anchor = new GameObject("Anchor");
        Anchor.transform.position = startPos;
        detectRange = 5f;
        target = Anchor;
        onCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canUpdate) return;
        if(!DestinationScript.isGameOver && canMove){
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
        else if(target == Anchor)
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
        if ((target != miloScript.gameObject && Vector3.Distance(startPos, miloScript.transform.position) <= maxChaseRange && !miloScript.isSneaking && Vector3.Distance(transform.position, miloScript.transform.position) <= detectRange) || (target == miloScript.gameObject && Vector3.Distance(startPos, miloScript.transform.position) <= maxChaseRange)
        ) 
        {
            target = miloScript.gameObject;
        } 
        else
        {
            target = Anchor;
        }
        FollowPath();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("Collision detected with " + other.gameObject.name);
        if(other.gameObject.CompareTag("Milo") && !onCooldown)
        {
            onCooldown = true;
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
        // Debug.Log("Collision detected with " + other.gameObject.name);
        if(other.gameObject.CompareTag("Milo") && !onCooldown)
        {
            onCooldown = true;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireSphere(startPos, maxChaseRange);
    }
}
