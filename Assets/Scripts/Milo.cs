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
        barkEffectRange = 5f;
        barkDisperseDistance = 1f;
        disperseSpeed = 5f;
    }



    void Movement(){
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
        transform.position += (movement * speed * Time.deltaTime);
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

        if(Input.GetKey(KeyCode.Q)){
            isBarking = true;
        } else{
            isBarking = false;
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
}