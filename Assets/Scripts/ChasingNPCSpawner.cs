using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingNPCSpawner : NPCSpawner
{
    // Start is called before the first frame update
    void Start()
    {
        maxNPCPerSpawner = 2;
        StartCoroutine(SpawnNPCs());
    }
}