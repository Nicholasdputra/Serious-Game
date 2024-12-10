using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPCSpawner : NPCSpawner
{
    // Start is called before the first frame update
    void Start()
    {
        maxNPCPerSpawner = 5;
        StartCoroutine(SpawnNPCs());
    }
}
