using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiloSFX : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] barkSFX;
    public AudioClip[] lickSFX;
    // public AudioClip[3] walkSFX;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void BarkSFX(){
        // Debug.Log("Bark SFX");
        int randomBark = Random.Range(0, barkSFX.Length);
        audioSource.PlayOneShot(barkSFX[randomBark]);
    }

    public void LickSFX(){
        // Debug.Log("Lick SFX");
        int randomLick = Random.Range(0, lickSFX.Length);
        audioSource.PlayOneShot(lickSFX[randomLick]);
    }
}
