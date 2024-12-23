using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationScript : MonoBehaviour
{
    Animator animator;
    [SerializeField] int NPCID;
    Vector2 movement;
    Vector2 lastMovement;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        GuardNPC guardScript = gameObject.GetComponent<GuardNPC>();
        ChasingNPC chasingScript = gameObject.GetComponent<ChasingNPC>();
        ChaseIfLookingNPC chaseIfLookingScript = gameObject.GetComponent<ChaseIfLookingNPC>();
        HaveToSneakNPC haveToSneakScript = gameObject.GetComponent<HaveToSneakNPC>();
        NPC npcScript = gameObject.GetComponent<NPC>();

        if(chasingScript != null){
            //chasing
            NPCID = 0;
        } else if (chaseIfLookingScript != null){
            NPCID = 1;
        } else if (guardScript != null){
            NPCID = 2;
        } else if (haveToSneakScript != null){
            NPCID = 3;
        } else if (npcScript != null){
            NPCID = 4;
        }else{
            NPCID = 5;
        }
        // Debug.Log(gameObject.name + " has NPCID: " + NPCID);
        animator.SetInteger("NPCIndex", NPCID);
    }

    private void Start() {
        if(NPCID == 5){
            StartCoroutine(StationaryNPC());
        }
    }

    IEnumerator StationaryNPC(){
        while(true){
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            int randomDirection = Random.Range(0, 4);
            switch(randomDirection){
                case 0:
                    movement = Vector2.up;
                    break;
                case 1:
                    movement = Vector2.down;
                    break;
                case 2:
                    movement = Vector2.left;
                    break;
                case 3:
                    movement = Vector2.right;
                    break;
            }
            StopMoving();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DestinationScript.isPaused){
            animator.speed = 0;
        }else{
            animator.speed = 1;
        }
        
    }

    public void SetDirection(Vector3 direction){
        animator.SetBool("IsMoving", true);
        movement.x = direction.x;
        movement.y = direction.y;
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }
    public void StopMoving(){
        lastMovement = movement;
        animator.SetBool("IsMoving", false);
        animator.SetFloat("LastX", lastMovement.x);
        animator.SetFloat("LastY", lastMovement.y);
    }
}
