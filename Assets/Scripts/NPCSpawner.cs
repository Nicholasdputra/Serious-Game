using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    // public static NPCSpawner instance { get; set;}

    [Header("Parameters")]
    public static GameObject[] NPCs;
    private Coroutine spawnCoroutine;
    private float lowerEndForRandomSpawn = 5f;
    private float upperEndForRandomSpawn = 10f;
    public static int totalChasingNPCs;
    public int maxChasingNPCs;
    public GameObject npcPrefab;
    public float spawnDelay = 2f; // Delay between spawns
    public GameObject[] spawnPoints;
    public bool canSpawn = true;
    
    void Start()
    {
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject npc1 in NPCs)
        {
            foreach (GameObject npc2 in NPCs)
            {
                if (npc1 != npc2)
                {
                    Physics2D.IgnoreCollision(npc1.GetComponent<Collider2D>(), npc2.GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(npc2.GetComponent<Collider2D>(), npc1.GetComponent<Collider2D>());
                }
            }
        }

        spawnPoints = GameObject.FindGameObjectsWithTag("NPCSpawner");
        if(spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points found!");
        }
        totalChasingNPCs = 0;
    }

    void Update()
    {
        if(spawnPoints.Length != 0 && canSpawn)
        {
            // Debug.Log("Total NPCs: " + totalChasingNPCs);
            // Debug.Log("Max NPCs: " + maxChasingNPCs);
            // Debug.Log("Spawn Coroutine: " + spawnCoroutine);
            if(totalChasingNPCs < maxChasingNPCs && spawnCoroutine == null)
            {
                Debug.Log("Spawning NPC");
                totalChasingNPCs++;
                // spawnCoroutine = StartCoroutine(SpawnNPCs());
            }
        }
    }

    public IEnumerator SpawnNPCs()
    {
        yield return new WaitForSeconds(Random.Range(lowerEndForRandomSpawn, upperEndForRandomSpawn));
        GameObject spawnFrom = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Debug.Log("Spawning NPC from: " + spawnFrom.name);
        GameObject chasingNpc = Instantiate(npcPrefab, spawnFrom.transform.position, Quaternion.identity);
        chasingNpc.GetComponent<ChasingNPC>().waypointsToGoTo.Add(spawnFrom);
        foreach (GameObject npc1 in NPCs)
        {
            Physics2D.IgnoreCollision(chasingNpc.GetComponent<Collider2D>(), npc1.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(npc1.GetComponent<Collider2D>(), chasingNpc.GetComponent<Collider2D>());
        }
        spawnCoroutine = null;
    }
}