using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] int spawnerCount;
    [SerializeField] int totalNPCsOnField;
    [SerializeField] int[] npcsNearSpawner;
    [SerializeField] GameObject[] spawners;

    [Header("Parameters")]
    [SerializeField] int maxNPCCount;
    [SerializeField] int maxNPCPerSpawner;

    [Header("NPC Prefabs")]
    [SerializeField] GameObject guardPrefab;
    [SerializeField] GameObject chasingPrefab;
    [SerializeField] GameObject chaseAndLookPrefab;
    [SerializeField] GameObject normalPrefab;
    // [SerializeField] GameObject stationaryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
    }

    void Initialize()
    {
        spawnerCount = 0;
        totalNPCsOnField = 0;
        spawners = GameObject.FindGameObjectsWithTag("NPCSpawner");
        spawnerCount = spawners.Length;
        npcsNearSpawner = new int[spawnerCount];

        maxNPCCount = 100;
        maxNPCPerSpawner = 10;
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnNPCs()
    {
        while (totalNPCsOnField < maxNPCCount)
        {
            for (int i = 0; i < spawnerCount; i++)
            {
                if (npcsNearSpawner[i] < maxNPCPerSpawner)
                {
                    int randomNPC = Random.Range(1, 11);
                    GameObject npc = null;
                    if (randomNPC <= 3)
                    {
                        npc = Instantiate(chaseAndLookPrefab, spawners[i].transform.position, Quaternion.identity);
                    }
                    else if (randomNPC <= 5)
                    {
                        npc = Instantiate(chasingPrefab, spawners[i].transform.position, Quaternion.identity);
                    }
                    else if (randomNPC <= 8)
                    {
                        npc = Instantiate(normalPrefab, spawners[i].transform.position, Quaternion.identity);
                    }
                    else
                    {
                        npc = Instantiate(guardPrefab, spawners[i].transform.position, Quaternion.identity);
                    }
                    npcsNearSpawner[i]++;
                    totalNPCsOnField++;
                    // if (totalNPCsOnField < 50)
                    // {
                    //     continue;
                    // }
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}
