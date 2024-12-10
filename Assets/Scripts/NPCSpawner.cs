using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCSpawner : MonoBehaviour
{
    [Header("Parameters")]
    public int npcsSpawned;
    public int maxNPCPerSpawner;
    public GameObject npcPrefab;
    public float spawnDelay = 2f; // Delay between spawns

    public IEnumerator SpawnNPCs()
    {
        for (int i = 0; i < maxNPCPerSpawner; i++)
        {
            Instantiate(npcPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}