using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    static BGM instance;
    AudioSource audioSource;
    string currentScene;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        // Debug.Log(instance.currentScene);
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Debug.Log("Destroying duplicate BGM");
            Destroy(gameObject);
            return;
        }
    }

    private void Update() {
        currentScene = SceneManager.GetActiveScene().name;
        if(currentScene != "MapLayout" && currentScene != "Tutorial"){
            if(!audioSource.isPlaying){
                audioSource.Play();
            }
        }else{
            Destroy(gameObject);
        }
    }
}
