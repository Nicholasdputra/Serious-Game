using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class ChaseIfLookingNPC : NPC
{
    public float radius;
    [Range(0, 360)]
    [SerializeField] float fovAngle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] private float maxRotationSpeed = 180f;
    [SerializeField] private Collider2D miloCollider;
    [SerializeField] private Vector2 directionToMilo;
    public bool isInFOV = false;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        canMove = true;
        target = waypointsToGoTo[0];
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

        if(recalculatePath == null){
            recalculatePath = StartCoroutine(ReFindPath());
        }

        miloCollider = Physics2D.OverlapCircle(transform.position, radius, targetMask);

        if(target == miloScript && Vector3.Distance(transform.position, target.transform.position) > 10f)
        {
            recalculateDelay = 0.6f;
        }
        else if(target == miloScript)
        {
            recalculateDelay = 0.2f;
        } else{
            recalculateDelay = 0.8f;
        }

        if(miloCollider != null){
            // FaceMilo();
            CheckIfMiloIsInFOV();
            if(isInFOV && canMove){
                target = miloScript.gameObject;
            } else{
                isInFOV = false;
                if(target == miloScript.gameObject){
                    target = waypointsToGoTo[0];
                    // FaceTarget(target.transform.position);
                }
            }
        } 
        else
        {
            isInFOV = false;
            if(target == miloScript.gameObject){
                target = waypointsToGoTo[0];
                // FaceTarget(target.transform.position);
            }
            // FaceTarget(target.transform.position);
        }

        if(canMove && !DestinationScript.isGameOver)
        {
            FollowPath();
        }
    }

    private void FaceMilo(){
        if(miloCollider != null){
            // Direction to target
            directionToMilo = (miloCollider.transform.position - transform.position).normalized;

            //Turn to Milo if in radius
            // Calculate the angle to face Milo
            float angleToFace = Mathf.Atan2(directionToMilo.y, directionToMilo.x) * Mathf.Rad2Deg;

            // Create the target rotation with the calculated angle
            Quaternion desiredRotation = Quaternion.Euler(new Vector3(0, 0, angleToFace));

            // Apply the rotation smoothly
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, maxRotationSpeed * Time.deltaTime);
        }
    }

    private void FaceTarget(Vector3 targetPosition)
    {
        // Debug.Log("Facing target");
        if(transform.position == targetPosition)
        {
            Debug.Log("Target reached");
            return;
        }
        Vector3 direction = (targetPosition - transform.position).normalized;
        // Debug.Log("Direction: " + direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Debug.Log("Angle: " + angle);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // Debug.Log("Rotation: " + rotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxRotationSpeed * Time.deltaTime);
        // Debug.Log("Transform.rotation: " + transform.rotation);
    }

    private void CheckIfMiloIsInFOV(){
        if(Vector2.Angle(transform.right, directionToMilo) < fovAngle / 2)
        {
            float distanceToMilo = Vector2.Distance(transform.position, miloCollider.transform.position);
            if(!Physics2D.Raycast(transform.position, directionToMilo, distanceToMilo, obstructionMask))
            {
                // Debug.Log("Milo is in FOV");
                isInFOV = true;
            }
            else
            {
                isInFOV = false;
            }
        }
        else
        {
            isInFOV = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Draw field of view
        Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle / 2, transform.forward) * transform.right * radius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle / 2, transform.forward) * transform.right * radius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if(!isInFOV)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(transform.position, (miloScript.transform.position - transform.position).normalized * radius);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Milo") && target == miloScript.gameObject)
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            quickTime.qteNPC = this;
            if(!QuickTime.isQuickTimeActive)
            {
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
            if(quickTime.qteNPC == null)
            {
                Debug.Log("qteNPC is null, called from ChaseIfLookingNPC");
            }
            if(!QuickTime.isQuickTimeActive)
            {
                quickTime.StartQuickTimeEvent();
                // Destroy(gameObject);
            }
        }
    }
}