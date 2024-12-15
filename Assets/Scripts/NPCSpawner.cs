using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public static NPCSpawner instance { get; set;}

    [Header("Parameters")]
    private float lowerEndForRandomSpawn = 10f;
    private float upperEndForRandomSpawn = 21f;
    public int totalChasingNPCs;
    public int maxChasingNPCs;
    public GameObject npcPrefab;
    public float spawnDelay = 2f; // Delay between spawns
    public GameObject[] spawnPoints;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of DestinationScript detected!");
        }
        spawnPoints = GameObject.FindGameObjectsWithTag("NPCSpawner");
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found!");
        }
        totalChasingNPCs = 0;
    }

    void Update()
    {
        if(spawnPoints.Length != 0)
        {
            if(totalChasingNPCs <= maxChasingNPCs)
            {
                totalChasingNPCs++;
                StartCoroutine(SpawnNPCs());
            }
        }
    }

    public IEnumerator SpawnNPCs()
    {
        yield return new WaitForSeconds(Random.Range(lowerEndForRandomSpawn, upperEndForRandomSpawn));
        GameObject spawnFrom = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject chasingNpc = Instantiate(npcPrefab, spawnFrom.transform.position, Quaternion.identity);
        chasingNpc.GetComponent<ChasingNPC>().waypointsToGoTo.Add(spawnFrom);
    }
}