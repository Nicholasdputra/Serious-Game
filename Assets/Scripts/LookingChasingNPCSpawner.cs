using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingChasingNPCSpawner : NPCSpawner
{
    // Start is called before the first frame update
    void Start()
    {
        maxNPCPerSpawner = 3;
        StartCoroutine(SpawnNPCs());
    }
}
