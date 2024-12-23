using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class DestinationScript : MonoBehaviour
{
    public static bool isGameOver;
    public static bool isPaused;
    public static int distractedCounter;
    [Header ("Level Done")] 
    // public bool isGameOver;
    public GameObject milo;
    public GameObject james;
    public int time;
    public GameObject gameoverPanel;
    public TMP_Text totalTime;
    //time in level
    public TMP_Text currentTime;
    public TMP_Text timesDistracted;
    public GameObject pausePanel;
    public static bool canEndLevel;

    void Awake()
    {
        canEndLevel = true;
        milo = GameObject.FindWithTag("Milo");
        james = GameObject.FindWithTag("James");
        milo.GetComponent<Milo>().destinationScript = this;
        distractedCounter = 0;
        isPaused = false;
        isGameOver = false;
        pausePanel.SetActive(false);
        StartCoroutine(Timer());
        time = 0;
        distractedCounter = 0;
        currentTime.text = "00:00:00";
    }

    void Start()
    {
        LoadScene();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            }else{
                PauseGame();
            }
        }
    }

    void LoadScene(){
        int levelIndex = SceneLoader.levelIndex;
        if(levelIndex == 1){
            transform.position = new Vector3(138,-89,0);
            transform.position = new Vector3(138,-89,0);
            milo.transform.position = new Vector3(75, -39, 0);
            james.transform.position = new Vector3(73, -30, 0);
        } else if(levelIndex == 2){
            transform.position = new Vector3(-116.5f,20.5f,0);
            milo.transform.position = new Vector3(-135, -89, 0);
            james.transform.position = new Vector3(-138, -90, 0);
        } else if(levelIndex == 3){
            transform.position = new Vector3(99.5f,43.5f,0);
            milo.transform.position = new Vector3(-115.5f, 20, 0);
            james.transform.position = new Vector3(-117.5f, 19.5f, 0);
        } else if(levelIndex == 0){
            transform.position = new Vector3(5,-2,0);
            milo.transform.position = new Vector3(-9,-4, 0);
            james.transform.position = new Vector3(-7,-4, 0);
        }
    }

    private void ShowEndScreen(){
        gameoverPanel.SetActive(true);
        totalTime.text = currentTime.text;
        timesDistracted.text = distractedCounter.ToString();
    }

    public IEnumerator Timer(){
        while(isGameOver == false){    
            if(!isPaused){
                yield return new WaitForSecondsRealtime(1f);
                time++;
                int hours = time / 3600;
                int minutes = (time % 3600) / 60;
                int seconds = time % 60;
                currentTime.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
            }else{
                yield return null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("James") && other.gameObject.GetComponent<James>().hasKeys && isGameOver == false && canEndLevel){
            Debug.Log("James has reached the level target");
            StopAllCoroutines();
            isGameOver = true;
            ShowEndScreen();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("James") && other.gameObject.GetComponent<James>().hasKeys && isGameOver == false && canEndLevel){
            Debug.Log("James has reached the level target");
            StopAllCoroutines();
            isGameOver = true;
            ShowEndScreen();
        }
    }

    public void ResumeGame(){
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void PauseGame(){
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame(){
       SceneManager.LoadScene("MapLayout");
    }

    public void BackToMainMenu()
    {
        // sfxSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("MainMenu");
    }
}