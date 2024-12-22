using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiloSFX : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] barkSFX;
    // public AudioClip[3] walkSFX;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void BarkSFX(){
        int randomBark = Random.Range(0, barkSFX.Length);
        audioSource.PlayOneShot(barkSFX[randomBark]);
    }
}
