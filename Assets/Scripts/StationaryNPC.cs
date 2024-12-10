using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryNPC : NPC
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
