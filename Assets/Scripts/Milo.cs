using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milo : MonoBehaviour
{
    public GameObject james;
    public James jamesScript;
    
    public bool canMove;
    public float speed;

    public bool isPullingJames;
    public float checkRangeForJames;
    public float mandatoryDistance;
    public float pullSpeed;

    public bool isBarking;
    public float barkEffectRange;
    public float barkDisperseDistance;
    public float disperseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Actions();
    }

    void Initialize(){
        isBarking = false;
        canMove = true;
        speed = 5f;

        checkRangeForJames = 2f;
        pullSpeed = 2f;
        mandatoryDistance = 1.25f;
        james = GameObject.FindWithTag("James");
        jamesScript = james.GetComponent<James>();
        
        isBarking = false;
        barkEffectRange = 1.5f;
        // barkDisperseDistance = 1f;
        disperseSpeed = 10f;
    }

    void Movement(){
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
        transform.position += movement * speed * Time.deltaTime;
    }

    bool CheckForJames(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRangeForJames);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("James"))
            {
                // Debug.Log("James is in range");
                return true;
            }
        }
        return false;
    }
    
    void Actions(){
        if(Input.GetKey(KeyCode.E) && CheckForJames()){
            isPullingJames = true;
            Pull();
        }

        if(Input.GetKeyDown(KeyCode.Q) && !isBarking){
            StartCoroutine(Bark());
        } 
    }

    void Pull(){
        //Guide james
        Debug.Log("Is Pulling James");
        float distance = Vector3.Distance(jamesScript.transform.position, transform.position);
        
        if (distance > mandatoryDistance)
        {
            // Move James' position closer to this object's position
            jamesScript.transform.position = Vector3.Lerp(jamesScript.transform.position, transform.position, pullSpeed * Time.deltaTime);
            // Debug.Log("Is Pulling James");
        }
        else
        {
            Debug.Log("James is close enough, stopping pull.");
        }
    }

    [ContextMenu("Bark")]
    IEnumerator Bark(){
        Debug.Log("Barking");
        isBarking = true;
        // Disperse all objects within a certain range
        //Get all colliders within range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, barkEffectRange);
        //draw circle for debugging
        // DebugCircle(transform.position, Vector3.forward, Color.red, barkEffectRange, 0f);
        foreach (Collider2D collider in colliders)
        {
            if(collider.CompareTag("NPC"))
            {
                NPC npc = collider.gameObject.GetComponent<NPC>();
                npc.canMove = false;
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                npc.rb.AddForce(direction * disperseSpeed, ForceMode2D.Impulse);
            }
        }
        yield return new WaitForSeconds(1f);
        foreach (Collider2D collider in colliders)
        {
            if(collider.CompareTag("NPC"))
            {
                NPC npc = collider.gameObject.GetComponent<NPC>();
                npc.canMove = true;
                npc.rb.velocity = Vector2.zero;
                npc.rb.angularVelocity = 0f;
            }
        }
        isBarking = false;  
    }

    void OnDrawGizmos()
    {
        // Draw a red circle to represent the bark effect range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, barkEffectRange);
    }
}