using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class James : MonoBehaviour
{
    Rigidbody2D rb;
    public bool hasKeys = true;
    bool isDroppingKeys = false;
    [SerializeField] GameObject keyPrefab;

    // Start is called before the first frame update
    void Start()
    {
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
        // Vector3 previousPosition = transform.position;
        yield return new WaitForSeconds(0 + Random.Range(0,10));
        Debug.Log("James is dropping the keys");
        DropKeys();
    }

    [ContextMenu("DropKeys")]
    public void DropKeys(){
        hasKeys = false;
        isDroppingKeys = false;
        Debug.Log("Spawning keys");
        for(int i = 0; i < 1; i++){
            bool droppedKeys = false;
            while(!droppedKeys){
                Vector2 spawnPos =  (Vector2)transform.position + Random.insideUnitCircle.normalized;
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
        Debug.Log("James dropped the keys!!! Stupid James");
    }
}
