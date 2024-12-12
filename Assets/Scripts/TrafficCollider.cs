using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrafficCollider : MonoBehaviour
{
    [HideInInspector] public TrafficLights trafficLights;
    [HideInInspector] public bool isVertical;
    public bool hasPlayer = false;
    BoxCollider2D boxCollider;

    private void Awake() {
        BoxCollider2D[] boxColliders;
        boxColliders = GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D bc in boxColliders){
            if(!bc.isTrigger){
                boxCollider = bc;
            }
        }
    }

    public void ChangeLights(){
        if(!hasPlayer){
            if(isVertical){
                if(trafficLights.counter % 2 == 0){
                    boxCollider.enabled = true;
                    GetComponent<SpriteRenderer>().enabled = true;
                }else{
                    boxCollider.enabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                }
            }else{
                if(trafficLights.counter % 2 == 0){
                    boxCollider.enabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                }else{
                    boxCollider.enabled = true;
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }else{
            Debug.Log("starting coroutine");
            StartCoroutine(ChangeLightsDelay());
        }
    }

    IEnumerator ChangeLightsDelay(){
        while(hasPlayer){
            Debug.Log("player inside, coroutine running");
            yield return null;
        }
        Debug.Log("player left");
        if(isVertical){
            if(trafficLights.counter % 2 == 0){
                boxCollider.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
            }else{
                boxCollider.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }else{
            if(trafficLights.counter % 2 == 0){
                boxCollider.enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }else{
                boxCollider.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Milo") || other.CompareTag("James") || other.CompareTag("NPC")){
            hasPlayer = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Milo") || other.CompareTag("James") || other.CompareTag("NPC")){
            hasPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Milo") || other.CompareTag("James") || other.CompareTag("NPC")){
            hasPlayer = false;
        }
    }
}
