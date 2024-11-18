using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChasingNPC : NPC
{
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
