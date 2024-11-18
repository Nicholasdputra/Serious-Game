using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChasingNPC : NPC
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        target = milo.gameObject;
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
        if(other.gameObject.CompareTag("Milo") && target == milo.gameObject)
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            quickTime.qteNPC = this;
            if(!quickTime.isQuickTimeActive){
                quickTime.StartQuickTimeEvent();
                // Destroy(gameObject);
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Milo") && target == milo.gameObject)
        {
            Debug.Log("Milo has been caught!");
            QuickTime quickTime = other.gameObject.GetComponent<QuickTime>();
            quickTime.qteNPC = this;
            if(quickTime.qteNPC == null){
                Debug.Log("qteNPC is null, called from ChasingNPC");
            }
            if(!quickTime.isQuickTimeActive){
                quickTime.StartQuickTimeEvent();
                // Destroy(gameObject);
            }
        }
    }
}
