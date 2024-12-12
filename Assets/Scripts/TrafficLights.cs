using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour
{
    List<GameObject> verticalCrossing;
    List<GameObject> horizontalCrossing;
    public int greenLightTimer = 20;
    public int counter = 0;
    // Start is called before the first frame update

    void Awake(){
        counter = 0;
        verticalCrossing = new List<GameObject>();
        horizontalCrossing = new List<GameObject>();
        foreach(Transform child in transform){
            child.GetComponent<TrafficCollider>().trafficLights = this;
            if(child.name == "VerticalCrossing"){
                verticalCrossing.Add(child.gameObject);
                child.GetComponent<TrafficCollider>().isVertical = true;
            }else if(child.name == "HorizontalCrossing"){
                horizontalCrossing.Add(child.gameObject);
                child.GetComponent<TrafficCollider>().isVertical = false;
            }
        }

        ChangeLights();
        StartCoroutine(LightTimer());
    }
    void Start()
    {
        
    }

    IEnumerator LightTimer(){
        while(true){
            yield return new WaitForSeconds(greenLightTimer);
            counter++;
            ChangeLights();
        }
    }

    void ChangeLights(){
        if(counter%2 == 0){
            foreach(GameObject go in verticalCrossing){
                go.GetComponent<TrafficCollider>().ChangeLights();
            }
            foreach(GameObject go in horizontalCrossing){
                go.GetComponent<TrafficCollider>().ChangeLights();
            }
        }else{
           foreach(GameObject go in verticalCrossing){
                go.GetComponent<TrafficCollider>().ChangeLights();
            }
            foreach(GameObject go in horizontalCrossing){
                go.GetComponent<TrafficCollider>().ChangeLights();
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
