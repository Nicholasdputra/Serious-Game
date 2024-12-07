using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseIfLookingNPC : NPC
{
    public float radius;
    [Range(0, 360)]
    public float fovAngle;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float maxRotationSpeed = 180f;
    [SerializeField] private Collider2D miloCollider;
    [SerializeField] private Vector2 directionToMilo;
    public bool isInFOV = false;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        canMove = true;
        StartCoroutine(FOVCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        miloCollider = Physics2D.OverlapCircle(transform.position, radius, targetMask);
        if(miloCollider != null){
            FaceMilo();
            CheckIfMiloIsInFOV();
            if(isInFOV){
                target = miloScript.gameObject;
            } else{
                isInFOV = false;
                if(target == miloScript.gameObject){
                    target = setDestination[Random.Range(0, setDestination.Length)];
                }
            }
        } 
        else
        {
            isInFOV = false;
            if(target == miloScript.gameObject){
                target = setDestination[Random.Range(0, setDestination.Length)];
            }
        }
        if(canMove && !DestinationScript.instance.isGameOver){
            FollowPath();
        }
    }

    private IEnumerator FOVCoroutine(){
        while(true){
            yield return new WaitForSeconds(delay);
            
            if(miloCollider != null){
                
            }
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
}
