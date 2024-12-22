using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamesSFX : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] keySFX;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void KeySFX(){
        int randomKey = Random.Range(0, keySFX.Length);
        audioSource.PlayOneShot(keySFX[randomKey]);
    }
}
