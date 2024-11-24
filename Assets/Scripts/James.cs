using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class James : MonoBehaviour
{
    public bool inLevel;
    Rigidbody2D rb;
    public bool hasKeys = true;
    bool isDroppingKeys = false;
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
        if(hasKeys && !isDroppingKeys){
            Debug.Log("Dropping keys");
            isDroppingKeys = true;
            StartCoroutine(DroppingKeys());
        }
    }

    public IEnumerator DroppingKeys(){
        yield return new WaitForSeconds(0 + Random.Range(0,10));
        Debug.Log("James is dropping the keys");
        DropKeys();
    }

    [ContextMenu("DropKeys")]
    void DropKeys(){
        hasKeys = false;
        Debug.Log("Spawning keys");
        for(int i = 0; i < 1; i++){
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
        }
        isDroppingKeys = false;
        Debug.Log("James dropped the keys!!! Stupid James");
    }
}