using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class James : MonoBehaviour
{
    public float pullSpeed;
    public int anxiety;
    public bool inLevel;
    Rigidbody2D rb;
    public bool hasKeys = true;
    [SerializeField] bool isDroppingKeys = false;
    public GameObject levelTarget;
    [SerializeField] GameObject keyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        inLevel = true;
        
        levelTarget = GameObject.FindWithTag("LevelTarget");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!DestinationScript.instance.isGameOver){
            if(hasKeys && !isDroppingKeys){
                // Debug.Log("Dropping keys");
                isDroppingKeys = true;
                StartCoroutine(DroppingKeys());
            }
        }

        //set pull speed based on anxiety
        //the higher anxiety the slower
    }

    public IEnumerator DroppingKeys(){
        Debug.Log("James is dropping the keys");
        yield return new WaitForSeconds(30 + Random.Range(0,10));
        // Debug.Log("James is dropping the keys");
        DropKeys();
    }

    [ContextMenu("DropKeys")]
    void DropKeys(){
        hasKeys = false;
        Debug.Log("Spawning keys");
        bool droppedKeys = false;
        while(!droppedKeys){
            Vector2 spawnPos =  (Vector2)transform.position + Random.insideUnitCircle.normalized;
            // Debug.DrawLine(transform.position, spawnPos, Color.red, 1f);
            //check if there is a colider in the spawn position
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.05f);
            if(hit == null){
                Instantiate(keyPrefab, spawnPos, Quaternion.identity);
                break;
            }else{
                Debug.Log("There is a collider in the spawn position");
            }
        }
        //Play key drop sound
        isDroppingKeys = false;
        Debug.Log("James dropped the keys!!! Stupid James");
    }
}