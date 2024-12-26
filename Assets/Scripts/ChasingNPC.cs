using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChasingNPC : NPC
{
    public Coroutine coutdownTillDestruction;
    [SerializeField] int timeLeftTillDestruction = 15;
    [SerializeField] float checkRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        usingSpeed = miloScript.defaultSpeed + 0.2f;
        coutdownTillDestruction = null;
        target = miloScript.gameObject;
        StartCoroutine(ReFindPath());
    }

    // Update is called once per frame
    void Update()
    {   
        if (!canUpdate) return;
        // if(path == null || path.Count == 0){
        //     Debug.Log("Path is null or path count is 0.");
        //     recalculatePath = null;
        //     recalculatePath = StartCoroutine(ReFindPath());
        // }
        if(target == miloScript && Vector3.Distance(transform.position, target.transform.position) > 10f)
        {
            recalculateDelay = 0.6f;
        }
        else if(target == miloScript)
        {
            recalculateDelay = 0.2f;
        }

        if(canMove && !DestinationScript.isGameOver){
            FollowPath();
        }
        CheckIfCloseToMilo();

        if(target == waypointsToGoTo[0] && Vector3.Distance(transform.position, target.transform.position) < 0.5f){
            Debug.Log(this + "has reached the waypoint, destroying it.");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Milo") && target == miloScript.gameObject)
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
        if(other.gameObject.CompareTag("Milo") && target == miloScript.gameObject)
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            quickTime.qteNPC = this;
            if(quickTime.qteNPC == null){
                Debug.Log("qteNPC is null, called from ChasingNPC");
            }
            if(!QuickTime.isQuickTimeActive){
                quickTime.StartQuickTimeEvent();
            }
        }
    }

    private void CheckIfCloseToMilo(){
        if(Vector2.Distance(transform.position, miloScript.transform.position) <= checkRange && coutdownTillDestruction == null){
            Debug.Log("Close to Milo, starting countdown for NPC " + this);
            coutdownTillDestruction = StartCoroutine(CountdownTillTimeout());
        }
    }

    private IEnumerator CountdownTillTimeout(){
        while(timeLeftTillDestruction > 0){
            yield return new WaitForSeconds(1);
            timeLeftTillDestruction--;
        }

        if(timeLeftTillDestruction <= 0){
            NPCSpawner.totalChasingNPCs--;
            target = waypointsToGoTo[0];
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRange);
    }
}