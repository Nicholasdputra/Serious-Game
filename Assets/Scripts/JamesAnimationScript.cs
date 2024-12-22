using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamesAnimationScript : MonoBehaviour
{
    Animator animator;
    Vector2 movement;
    Vector2 lastMove;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 direction){
        movement = direction;
        if (!DestinationScript.isPaused && !DestinationScript.isGameOver && !QuickTime.isQuickTimeActive)
        {
            animator.speed = 1;
            if(movement == Vector2.zero)
            {
                animator.SetBool("isMoving", false);
                animator.SetFloat("LastX", lastMove.x);
                animator.SetFloat("LastY", lastMove.y);
            }else{
                lastMove = movement; 
                animator.SetBool("isMoving", true);
                animator.SetFloat("MoveX", movement.x);
                animator.SetFloat("MoveY", movement.y);
            }
        }else if(DestinationScript.isPaused){
            animator.speed = 0;
        }
    }
}
