using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public bool isPaused;
    bool isMoving;
    Animator animator;
    Vector2 movement;
    Vector2 lastMove;
    float idleDelay = 0.5f;
    bool isRunningCoroutine = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (!isPaused && !DestinationScript.isGameOver && !QuickTime.isQuickTimeActive)
        {
            // Debug.Log("SpriteAnimation");
            if(movement == Vector2.zero)
            {
                animator.SetBool("isMoving", false);
                isMoving = false;
                if(!isRunningCoroutine){
                    StartCoroutine(Timer());
                }
                animator.SetFloat("LastX", lastMove.x);
                animator.SetFloat("LastY", lastMove.y);
            }else{
                animator.SetBool("isMoving", true);
                isMoving = true;
                lastMove = movement; 
                StopCoroutine(Timer());
                isRunningCoroutine = false;
                animator.SetFloat("MoveX", movement.x);
                animator.SetFloat("MoveY", movement.y);
            }
        }
    }

    IEnumerator Timer()
    {
        isRunningCoroutine = true;
        // Debug.Log("Timer");
        idleDelay = 0;
        while(!isMoving){
            // Debug.Log("IdleDelay: " + idleDelay);
            idleDelay += Time.deltaTime;
            animator.SetFloat("IdleDelay", idleDelay);
            yield return null;
        }
    }

}