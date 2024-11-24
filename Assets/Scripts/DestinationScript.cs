using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DestinationScript : MonoBehaviour
{
    [Header ("Level Done")] 
    public James james;
    public bool inLevel;
    public int time;
    public GameObject gameoverPanel;
    public int distractedCounter;
    public TMP_Text totalTime;
    //time in level
    public TMP_Text currentTime;
    public TMP_Text timesDistracted;

    void Start()
    {
        inLevel = true;
        StartCoroutine(Timer());
        time = 0;
        distractedCounter = 0;
        currentTime.text = "00:00:00";
    }

    private void ShowEndScreen(){
        inLevel = false;
        gameoverPanel.SetActive(true);
        totalTime.text = currentTime.text;
        timesDistracted.text = distractedCounter.ToString();
    }

    public IEnumerator Timer(){
        while(inLevel){    
            yield return new WaitForSecondsRealtime(1f);
            time++;
            int hours = time / 3600;
            int minutes = (time % 3600) / 60;
            int seconds = time % 60;
            currentTime.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("James") && other.gameObject.GetComponent<James>().hasKeys){
            Debug.Log("James has reached the level target");
            ShowEndScreen();
        }
    }
}