using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milo : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject james;
    public James jamesScript;
    public bool canMove;
    public float defaultSpeed
    public float speed;
    public bool hasKey;
    public bool isPullingJames;
    public float checkRangeForJames;
    public float checkRangeForItems;
    public float mandatoryDistance;
    public float pullSpeed;
    public bool isBarking;
    public float barkEffectRange;
    public float barkDisperseDistance;
    public float disperseSpeed;
    public bool canShowDirection;
    public bool canLick;
    public GameObject arrowPrefab;
    public float arrowSpawnOffset;
    Vector2 movement;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    void Initialize(){
        isBarking = false;
        canMove = true;
        speed = defaultSpeed;;

        checkRangeForJames = 2f;
        checkRangeForItems = 1f;
        pullSpeed = 2f;
        mandatoryDistance = 1.25f;
        james = GameObject.FindWithTag("James");
        jamesScript = james.GetComponent<James>();
        
        isBarking = false;
        canLick = true;
        barkEffectRange = 1.5f;
        // barkDisperseDistance = 1f;
        disperseSpeed = 10f;
        
        arrowSpawnOffset = 1.5f;
        canShowDirection = true;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if(!DestinationScript.instance.isGameOver){
            Actions();
        }
    }

    private void FixedUpdate() {
        if(isPullingJames){
            speed = jamesScript.pullSpeed;
        }
        else{
            speed = defaultSpeed;
        }
        rb.velocity = movement.normalized * speed;
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

    void CheckForItems(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRangeForItems);
        foreach (Collider2D collider in colliders)
        {
            if(collider.CompareTag("Key"))
            {
                // Debug.Log("Keys are in range");
                hasKey = true;
                Destroy(collider.gameObject);
            }
        }
    }
    
    void Actions(){
        bool jamesInRange = CheckForJames();
        if(Input.GetKey(KeyCode.E) ){
            if(jamesInRange){    
                isPullingJames = true;
                Pull();
            }            
        }else{
            isPullingJames = false;
        }

        if(Input.GetKeyDown(KeyCode.Q) && !isBarking){
            StartCoroutine(Bark());
        } 
        
        if(Input.GetKeyDown(KeyCode.F)){
            CheckForItems();
            if(jamesInRange){
                //if james in range, give items to james
                if(hasKey){
                    jamesScript.hasKeys = true;
                    hasKey = false;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.C) && canShowDirection){
            canShowDirection = false;
            //maybe call anim and call coroutine from the anim (sniffing, terus muncul arrow)
            StartCoroutine(ShowDirection());
        }

        if(Input.GetKeyDown(KeyCode.M) && canLick && jamesInRange){
            StartCoroutine(Lick());
        }
    }

    void Pull(){
        //Guide james
        // Debug.Log("Is Pulling James");
        float distance = Vector3.Distance(jamesScript.transform.position, transform.position);
        
        if (distance > mandatoryDistance)
        {
            // Move James' position closer to this object's position
            jamesScript.transform.position = Vector3.Lerp(jamesScript.transform.position, transform.position, pullSpeed * Time.deltaTime);
            //Change James animator variable
            // Debug.Log("Is Pulling James");
        }
        else
        {
            // Debug.Log("James is close enough, stopping pull.");
        }
    }

    [ContextMenu("Bark")]
    IEnumerator Bark(){
        Debug.Log("Barking");
        isBarking = true;
        // Disperse all objects within a certain range
        //Get all colliders within range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, barkEffectRange);
        List<NPC> npcList = new List<NPC>();
        foreach(Collider2D collider in colliders){
            if(collider.CompareTag("NPC")){
                // Debug.Log("NPC in range");
                npcList.Add(collider.GetComponent<NPC>());
            }
        }
        
        //draw circle for debugging
        // DebugCircle(transform.position, Vector3.forward, Color.red, barkEffectRange, 0f);
        foreach (NPC npc in npcList)
        {
            npc.canMove = false;
            Vector3 direction = (npc.transform.position - transform.position).normalized;
            npc.rb.AddForce(direction * disperseSpeed, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.5f);
        
        foreach (NPC npc in npcList)
        {

            npc.rb.velocity = Vector2.zero;
            npc.rb.angularVelocity = 0f;
        }

        yield return new WaitForSeconds(0.25f);

        foreach (NPC npc in npcList)
        {
            npc.canMove = true;
        }

        yield return new WaitForSeconds(0.25f);
        
        isBarking = false;  
    }

    IEnumerator ShowDirection(){
        //find direction from milo to james' levelTarget
        Vector3 direction = (jamesScript.levelTarget.transform.position - transform.position).normalized;
        //instantiate an arrow prefab pointing in that direction
        Vector3 spawnPos = transform.position + direction * arrowSpawnOffset;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.Euler(0, 0, angle-90f));
        yield return new WaitForSeconds(3f);
        Destroy(arrow);
        yield return new WaitForSeconds(5f);
        canShowDirection = true;
    }

    IEnumerator Lick(){
        canLick = false;
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        jamesScript.anxiety -= 10;
        speed = defaultSpeed;
        canLick = true;
    }

    void OnDrawGizmos()
    {
        // Draw a red circle to represent the bark effect range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, barkEffectRange);
    }
}