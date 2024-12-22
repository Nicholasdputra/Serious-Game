using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    BGM instance;
    AudioSource audioSource;
    string currentScene;

    private void Awake() {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        currentScene = SceneManager.GetActiveScene().name;
        if(currentScene != "MapLayout"){
            if(!audioSource.isPlaying){
                audioSource.Play();
            }
        }else{
            Destroy(gameObject);
        }
    }
}
