using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Milo : MonoBehaviour
{
    public DestinationScript destinationScript;
    public Rigidbody2D rb;
    public GameObject james;
    public James jamesScript;
    JamesAnimationScript jamesAnimationScript;
    public bool canMove;
    public float defaultSpeed;
    public float speed;
    public bool hasKey;
    public bool isPullingJames;
    public float checkRangeForJames;
    public float checkRangeForItems;
    public float mandatoryDistance;
    public float pullSpeed;
    public bool isBarking;
    // public bool canBark;
    bool isLicking;
    public float barkEffectRange;
    public float barkDisperseDistance;
    public float barkCooldown;
    public float disperseSpeed;
    public bool canShowDirection;
    public bool canLick;
    public GameObject arrowPrefab;
    public float arrowSpawnOffset;
    Vector2 movement;
    public bool isSneaking;
    public float sneakSpeed;
    public bool canRun;
    public bool isRunning;
    public float runSpeed;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    void Initialize(){
        // canBark = true;
        isBarking = false;
        canMove = true;
        canRun = true;
        isSneaking = false;
        isRunning = false;
        speed = defaultSpeed;

        checkRangeForJames = 2f;
        checkRangeForItems = 1f;
        pullSpeed = 3f;
        mandatoryDistance = 1.25f;
        james = GameObject.FindWithTag("James");
        jamesScript = james.GetComponent<James>();
        jamesScript.milo = this;
        jamesAnimationScript = james.GetComponent<JamesAnimationScript>();
        
        isBarking = false;
        isLicking = false;
        canLick = true;
        barkEffectRange = 4f;
        barkCooldown = 2.5f;
        // barkDisperseDistance = 1f;
        disperseSpeed = 15f;
        
        arrowSpawnOffset = 1.5f;
        canShowDirection = true;
        if(destinationScript == null){
            destinationScript = GameObject.FindWithTag("LevelTarget").GetComponent<DestinationScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        
        if(!DestinationScript.isGameOver){
            Actions();
        }
    }

    private void FixedUpdate() {
        if(isLicking){
            speed = 0f;
        }
        else if(isPullingJames){
            speed = jamesScript.pullSpeed;
        }
        else{
            speed = defaultSpeed;
        }
        if(canMove){
            rb.velocity = movement.normalized * speed;
        }
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
        if(Input.GetKey(KeyCode.E)){
            if(jamesInRange){
                jamesAnimationScript.Move(movement);
                isPullingJames = true;
                canRun = false;
                Pull();
            }            
        }else{
            isPullingJames = false;
            canRun = true;
            jamesAnimationScript.Move(Vector2.zero);
        }

        if(Input.GetKeyDown(KeyCode.Q) && !isBarking){
            Debug.Log("Bark");
            StartCoroutine(Bark());
        } 
        
        if(Input.GetKeyDown(KeyCode.F)){
            Debug.Log("Checking for items");
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
            Debug.Log("Showing Direction");
            canShowDirection = false;
            //maybe call anim and call coroutine from the anim (sniffing, terus muncul arrow)
            StartCoroutine(ShowDirection());
        }

        if(Input.GetKeyDown(KeyCode.X) && canLick && jamesInRange){
            Debug.Log("Licking");
            StartCoroutine(Lick());
        }

        if(Input.GetKey(KeyCode.LeftControl)){
            Debug.Log("Sneaking");
            speed = sneakSpeed;
            isRunning = false;
            isSneaking = true;
        } 
        else{
            speed = defaultSpeed;
            isSneaking = false;
        }

        if(Input.GetKey(KeyCode.LeftShift) && canRun){
            Debug.Log("Running");
            speed = runSpeed;
            isSneaking = false;
            isRunning = true;
        } 
        else{
            speed = defaultSpeed;
            isRunning = false;
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
            //Change James animator variable
            // jamesAnimationScript.Move((transform.position - jamesScript.transform.position).normalized);
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
        GetComponent<MiloSFX>().BarkSFX();
        // canBark = false;
        isBarking = true;
        // Disperse all objects within a certain range
        //Get all colliders within range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, barkEffectRange);
        List<NPC> npcList = new List<NPC>();
        foreach(Collider2D collider in colliders)
        {
            if(collider.CompareTag("NPC"))
            {
                // Debug.Log("NPC in range");
                npcList.Add(collider.GetComponent<NPC>());
            }
        }
        
        //draw circle for debugging
        // DebugCircle(transform.position, Vector3.forward, Color.red, barkEffectRange, 0f);
        foreach (NPC npc in npcList)
        {
            if(npc.gameObject.name.Contains("Stationary NPC")){
                continue;
            }
            npc.canMove = false;
            Vector3 direction = (npc.transform.position - transform.position).normalized;
            npc.rb.drag = 0.1f;
            npc.rb.AddForce(direction * disperseSpeed, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (NPC npc in npcList)
        {

            npc.rb.velocity = Vector2.zero;
            npc.rb.drag = 1000f;
            npc.rb.angularVelocity = 0f;
        }

        yield return new WaitForSeconds(0.25f);

        foreach (NPC npc in npcList)
        {
            npc.canMove = true;
        }

        yield return new WaitForSeconds(barkCooldown);
        isBarking = false;  
        // canBark = true;
    }

    [Header("Direction")]
    public float directionDuration = 3;
    public float directionCD = 5;
    IEnumerator ShowDirection(){
        //find direction from milo to james' levelTarget
        Vector3 direction = (jamesScript.levelTarget.transform.position - transform.position).normalized;
        //instantiate an arrow prefab pointing in that direction
        Vector3 spawnPos = transform.position + direction * arrowSpawnOffset;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.Euler(0, 0, angle-90f));
        yield return new WaitForSeconds(directionDuration);
        Destroy(arrow);
        yield return new WaitForSeconds(directionCD);
        canShowDirection = true;
    }

    [Header("Lick")]
    public float lickDuration = 0.5f;
    public float lickCD = 2f;
    IEnumerator Lick(){
        canLick = false;
        isLicking = true;
        speed = 0;
        yield return new WaitForSeconds(lickDuration);
        isLicking = false;
        jamesScript.anxiety -= 25;
        speed = defaultSpeed;
        yield return new WaitForSeconds(lickCD);
        canLick = true;
    }

    void OnDrawGizmos()
    {
        // Draw a red circle to represent the bark effect range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, barkEffectRange);
    }
}