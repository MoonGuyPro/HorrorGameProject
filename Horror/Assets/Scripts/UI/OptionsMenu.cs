using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject audioOptionsMenu;
    [SerializeField] private GameObject graphicsOptionsMenu;
    [SerializeField] private GameObject controlsOptionsMenu;

    private void Awake()
    {
        // FMOD stuff
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        ambienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Ambience");

        masterBus.setVolume(0.0f);
    }
    
    private void Start()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        ambienceBus.setVolume(ambienceVolume);

        // Resolutions dropdown setup
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentResIndex = 0;
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + "x" + resolutions[i].height;
            resolutionOptions.Add(resOption);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResIndex = i;
            }
                
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        
        // Load PlayerPrefs
        // Audio
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 0.8f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.8f);
        ambienceVolume = PlayerPrefs.GetFloat("ambienceVolume", 0.8f);

        // Controls
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 0.8f);
        invertYAxis = PlayerPrefs.GetInt("invertYAxis", 0) == 1;
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        ambienceBus.setVolume(ambienceVolume);
    }

    #region AudioSettings
    [Header("Audio")]
    public GameObject masterVolumeSlider;
    public GameObject masterVolumeLabel;
    public GameObject musicSlider;
    public GameObject musicVolumeLabel;
    public GameObject soundSlider;
    public GameObject soundVolumeLabel;
    public GameObject ambientSlider;
    public GameObject ambientVolumeLabel;
    
    FMOD.Studio.Bus masterBus;
    FMOD.Studio.Bus musicBus;
    FMOD.Studio.Bus sfxBus;
    FMOD.Studio.Bus ambienceBus;
    float masterVolume = 0.8f;
    float musicVolume = 0.8f;
    float sfxVolume = 0.8f;
    float ambienceVolume = 0.8f;
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        int value = (int)(masterVolume * 100.0f);
        masterVolumeLabel.GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString() + "%";
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        int value = (int)(musicVolume * 100.0f);
        musicVolumeLabel.GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString() + "%";
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        int value = (int)(sfxVolume * 100.0f);
        soundVolumeLabel.GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString() + "%";
    }
    
    public void SetAmbienceVolume(float volume)
    {
        ambienceVolume = volume;
        int value = (int)(ambienceVolume * 100.0f);
        ambientVolumeLabel.GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString() + "%";
    }
    
    public float GetMasterVolume()
    {
        return masterVolume;
    }

    #endregion
    
    #region GraphicsSettings
    
    [Header("Graphics")]
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    public void SetGraphicsQuality(int qualityIndex)
    {
        // Kris here - this probably won't work as expected since I don't know what's inside QualitySettings just yet
        // And I hope there's no need to use PlayerPrefs and Unity will remember user's choice, or I'm gonna be sad.
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    
    #endregion
    
    #region ControlsSettings
    [Header("Controls")]
    public GameObject sensitivitySlider;
    public GameObject sensitivityLabel;

    float sensitivity = 0.8f;
    bool invertYAxis = false;

    public void SetSensitivity(float newSens)
    {
        sensitivity = newSens;
        int value = (int)(sensitivity * 100.0f);
        sensitivityLabel.GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString() + "%";
    }
    
    public float GetSensitivity()
    {
        return sensitivity;
    }
    
    public void SetInvertYAxis(bool invert)
    {
        invertYAxis = invert;
    }
    
    public bool GetInvertYAxis()
    {
        return invertYAxis;
    }
    
    #endregion
    
    #region MenuNavigation
    
    public void OnAudioPressed()
    {
        audioOptionsMenu.SetActive(true);
        graphicsOptionsMenu.SetActive(false);
        controlsOptionsMenu.SetActive(false);
    }
    
    public void OnGraphicsPressed()
    {
        audioOptionsMenu.SetActive(false);
        graphicsOptionsMenu.SetActive(true);
        controlsOptionsMenu.SetActive(false);
    }
    
    public void OnControlsPressed()
    {
        audioOptionsMenu.SetActive(false);
        graphicsOptionsMenu.SetActive(false);
        controlsOptionsMenu.SetActive(true);
    }

    public void OnBackPressed()
    {
        PlayerPrefs.Save();
        Debug.Log("Preferences saved.");
        //SetActive(false);
        return;
    }
    #endregion
}
