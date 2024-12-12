using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrafficCollider : MonoBehaviour
{
    [HideInInspector] public TrafficLights trafficLights;
    [HideInInspector] public bool isVertical;
    private AStar_Grid grid;
    int entityCounter;
    BoxCollider2D boxCollider;

    private void Awake() {
        grid = GameObject.FindGameObjectWithTag("Pathfinding AI").GetComponent<AStar_Grid>();
        BoxCollider2D[] boxColliders;
        boxColliders = GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D bc in boxColliders){
            if(!bc.isTrigger){
                boxCollider = bc;
            }
        }
    }

    public void ChangeLights(){
        if(entityCounter == 0){
            if(isVertical){
            if(trafficLights.counter % 2 == 0){
                boxCollider.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                gameObject.layer = LayerMask.NameToLayer("Unwalkable");
            }else{
                boxCollider.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }else{
            if(trafficLights.counter % 2 == 0){
                boxCollider.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                gameObject.layer = LayerMask.NameToLayer("Default");
            }else{
                boxCollider.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                gameObject.layer = LayerMask.NameToLayer("Unwalkable");
            }
        }
        }else{
            Debug.Log("starting coroutine");
            StartCoroutine(ChangeLightsDelay());
        }
        grid.CreateGrid();
    }

    IEnumerator ChangeLightsDelay(){
        while(entityCounter != 0){
            Debug.Log("player inside, coroutine running");
            yield return null;
        }
        Debug.Log("player left");
        if(isVertical){
            if(trafficLights.counter % 2 == 0){
                boxCollider.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                gameObject.layer = LayerMask.NameToLayer("Unwalkable");
            }else{
                boxCollider.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }else{
            if(trafficLights.counter % 2 == 0){
                boxCollider.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                gameObject.layer = LayerMask.NameToLayer("Default");
            }else{
                boxCollider.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                gameObject.layer = LayerMask.NameToLayer("Unwalkable");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Milo") || other.CompareTag("James") || other.CompareTag("NPC")){
            entityCounter++;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Milo") || other.CompareTag("James") || other.CompareTag("NPC")){
            entityCounter--;
        }
    }
}
