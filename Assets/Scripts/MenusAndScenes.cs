using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class MenusAndScenesScript : MonoBehaviour
{
    public GameObject mainMenuButtonsPanel;

    [Header("Options Panel")]
    public GameObject optionsPanel;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public Slider musicSlider;
    public Slider sfxSlider;


    //Start Game
    public void StartGame()
    {
        // sfxSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("LevelSelect");
    }
    public void TestGame()
    {
        // sfxSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("MapLayout");
    }

    //Back to Main Menu
    public void BackToMainMenu()
    {
        // sfxSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel1()
    {
        DestinationScript.instance = null;
        // sfxSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        DestinationScript.instance = null;
        // sfxSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        DestinationScript.instance = null;
        // sfxSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("Level3");
    }

    //Options
    public void OpenOptions()
    {
        // sfxSource.PlayOneShot(clickClip);
        optionsPanel.SetActive(true);
        mainMenuButtonsPanel.SetActive(false);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (filteredResolutions == null || filteredResolutions.Count == 0)
        {
            Debug.LogError("Filtered resolutions list is empty.");
            return;
        }

        if (resolutionIndex < 0 || resolutionIndex >= filteredResolutions.Count)
        {
            Debug.LogError($"Resolution index {resolutionIndex} is out of range. Valid range is 0 to {filteredResolutions.Count - 1}.");
            return;
        }

        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
    

    public void CloseOptions()
    {
        // sfxSource.PlayOneShot(clickClip);
        optionsPanel.SetActive(false);
        mainMenuButtonsPanel.SetActive(true);
    }

    //Quit
    public void QuitGame()
    {
        // sfxSource.PlayOneShot(clickClip);
        Application.Quit();
    }


    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            // bgmSource.Play();
            // bgmSource.loop = true;
            // Initialize the sliders with saved values (or default 1.0)
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

            // Attach listener functions to the sliders
            musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);

            // Set initial volume based on saved values
            // bgmSource.volume = musicSlider.value;
            // sfxSource.volume = sfxSlider.value;
            optionsPanel.SetActive(false);
            mainMenuButtonsPanel.SetActive(true);

            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
            resolutionDropdown.ClearOptions();
            currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
            
            for (int i = 0; i < resolutions.Length; i++)
            {
                if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate) 
                {
                    filteredResolutions.Add(resolutions[i]);
                } 
            }

            if (filteredResolutions.Count == 0)
            {
                filteredResolutions.AddRange(resolutions);
            }

            // Sort resolutions by width and height
            filteredResolutions = filteredResolutions.OrderByDescending(res => res.width).ThenByDescending(res => res.height).ToList();
            

            List<string> options = new List<string>();

            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value.ToString("0.##") + " Hz";
                options.Add(resolutionOption);
                
                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height && (float)filteredResolutions[i].refreshRateRatio.value == currentRefreshRate)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            if (filteredResolutions.Count > 0)
            {
                if (currentResolutionIndex < 0 || currentResolutionIndex >= filteredResolutions.Count)
                {
                    currentResolutionIndex = 0; // Default to the first resolution if index is out of range
                }
                resolutionDropdown.value = currentResolutionIndex;
                resolutionDropdown.RefreshShownValue();
                SetResolution(currentResolutionIndex);
            } 
            else
            {
                float currentRefreshRate;
                currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
                Debug.Log("Current Refresh Rate: " + currentRefreshRate.ToString("0.##") + " Hz");
                Debug.LogError("No resolutions available for the current refresh rate.");
            }
        }
    }

    private void OnMusicSliderValueChanged(float value)
    {
        bgmSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value); // Save the volume setting
    }
    
    // Update SFX volume when slider changes
    private void OnSFXSliderValueChanged(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value); // Save the volume setting
    }
}