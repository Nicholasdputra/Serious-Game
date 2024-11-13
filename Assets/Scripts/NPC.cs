using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Milo milo;

    [Header ("Pathing")]
    public bool canMove;
    public bool hasSetPath = false;
    public List<string> path = new List<string>();
    //U = Up, D = Down, L = Left, R = Right + value
    //Contoh: U2 = ke atas 2 unit
    public float speed = 1f;
    private Vector3 targetPosition;
    public bool isMoving = false;
    public bool canRandomizeDirection = true;
    private Vector3 direction;
    private Vector3 startPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize(){
        milo = GameObject.FindWithTag("Milo").GetComponent<Milo>();
    }

    // Update is called once per frame
    void Update()
    {
        if(milo.isBarking){
            Runaway();
        }
        
    }

    void Runaway(){
        float distance = Vector3.Distance(transform.position, milo.transform.position);
        if (distance <= milo.barkEffectRange)
        {
            Vector3 direction = transform.position - milo.transform.position;
            transform.position += direction.normalized * milo.disperseSpeed * Time.deltaTime;
        }
    }
}
