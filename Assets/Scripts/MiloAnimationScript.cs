using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public bool isPaused;
    Animator animator;
    Vector2 movement;
    Vector2 lastMove;
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
        if (!isPaused && !DestinationScript.instance.isGameOver && !QuickTime.isQuickTimeActive)
        {
            // Debug.Log("SpriteAnimation");
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
        }
    }
}
