using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public int spawnerCount;
    public int totalNPCsOnField;
    public int[] npcsNearSpawner;
    public GameObject[] spawners;

    [Header("NPC Prefabs")]
    public GameObject guardPrefab;
    public GameObject chasingPrefab;
    public GameObject chaseAndLookPrefab;
    public GameObject normalPrefab;
    public GameObject stationaryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
