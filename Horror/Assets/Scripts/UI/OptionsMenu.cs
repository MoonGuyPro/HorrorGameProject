using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject audioOptionsMenu;
    [SerializeField] private GameObject graphicsOptionsMenu;
    [SerializeField] private GameObject controlsOptionsMenu;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        // FMOD stuff
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        ambienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Ambience");
        uisfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/UISFX");
        
        // Load PlayerPrefs
        
        // Audio
        // moved these here from Update since they were triggered when the options were enabled first time,
        // not instantly when the game started, so there was a volume jump when the options were opened
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 0.8f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.8f);
        ambienceVolume = PlayerPrefs.GetFloat("ambienceVolume", 0.8f);

        // Controls
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 0.8f);
        invertYAxis = PlayerPrefs.GetInt("invertYAxis", 0) == 1;

        masterBus.setVolume(0.0f);
    }
    
    private void Start()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        ambienceBus.setVolume(ambienceVolume);
        
        // Hide everything (except audio) because Unity is stupid
        OnAudioPressed();

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
        
        // Brightness
        brightness.TryGetSettings(out exposure);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        ambienceBus.setVolume(ambienceVolume);
        uisfxBus.setVolume(uisfxVolume);
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        OnBackPressed();
    }

    #region AudioSettings
    [Header("Audio")]
    public GameObject masterVolumeLabel;
    public GameObject musicVolumeLabel;
    public GameObject soundVolumeLabel;
    public GameObject ambientVolumeLabel;
    public GameObject uisfxVolumeLabel;
    
    FMOD.Studio.Bus masterBus;
    FMOD.Studio.Bus musicBus;
    FMOD.Studio.Bus sfxBus;
    FMOD.Studio.Bus ambienceBus;
    FMOD.Studio.Bus uisfxBus;
    float masterVolume = 0.8f;
    float musicVolume = 0.8f;
    float sfxVolume = 0.8f;
    float ambienceVolume = 0.8f;
    float uisfxVolume = 0.8f;
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        int value = (int)(masterVolume * 100.0f);
        masterVolumeLabel.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
        PlayerPrefs.GetFloat("masterVolume", value);
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        int value = (int)(musicVolume * 100.0f);
        musicVolumeLabel.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
        PlayerPrefs.GetFloat("musicVolume", value);
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        int value = (int)(sfxVolume * 100.0f);
        soundVolumeLabel.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
        PlayerPrefs.GetFloat("sfxVolume", value);
    }
    
    public void SetAmbienceVolume(float volume)
    {
        ambienceVolume = volume;
        int value = (int)(ambienceVolume * 100.0f);
        ambientVolumeLabel.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
        PlayerPrefs.GetFloat("ambienceVolume", value);
    }
    
    public void SetUISFXVolume(float volume)
    {
        uisfxVolume = volume;
        int value = (int)(uisfxVolume * 100.0f);
        uisfxVolumeLabel.GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString() + "%";
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

    public GameObject brightnessSlider;
    public GameObject brightnessLabel;
    public PostProcessProfile brightness;
    //public PostProcessLayer layer; //not sure if needed but oh well

    private AutoExposure exposure;
    
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

    public void SetBrightness(float newBright)
    {
        exposure.keyValue.value = newBright;// < 0.05f ? 0.05f : newBright;
        int value = (int)(exposure.keyValue.value * 100.0f);
        Debug.Log(value);
        brightnessLabel.GetComponent<TextMeshProUGUI>().text = value + "%";
    }
    
    #endregion
    
    #region ControlsSettings
    [Header("Controls")]
    public GameObject sensitivitySlider;
    public GameObject sensitivityLabel;

    private float sensitivity = 0.8f;
    private bool invertXAxis = false;
    private bool invertYAxis = false;

    public void SetSensitivity(float newSens)
    {
        sensitivity = newSens;
        int value = (int)(sensitivity * 100.0f);
        sensitivityLabel.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
        PlayerPrefs.SetFloat("sensitivity", newSens);
    }
    
    public float GetSensitivity()
    {
        return sensitivity;
    }
    
    public void SetInvertXAxis(bool invert)
    {
        invertXAxis = invert;
        PlayerPrefs.SetInt("invertXAxis", invert ? 1 : 0);
    }
    
    public bool GetInvertXAxis()
    {
        return invertXAxis;
    }
    
    public void SetInvertYAxis(bool invert)
    {
        invertYAxis = invert;
        PlayerPrefs.SetInt("invertYAxis", invert ? 1 : 0);
    }
    
    public bool GetInvertYAxis()
    {
        return invertYAxis;
    }
    
    #endregion
    
    #region MenuNavigation

    public GameObject controlsOverviewMenu;
    public GameObject controlsOverviewButton;
    public GameObject returnToControlsButton;
    
    public void OnAudioPressed()
    {
        HideMenus();
        audioOptionsMenu.SetActive(true);
    }
    
    public void OnGraphicsPressed()
    {
        HideMenus();
        graphicsOptionsMenu.SetActive(true);
    }
    
    public void OnControlsPressed()
    {
        HideMenus();
        controlsOptionsMenu.SetActive(true);
    }

    public void OnControlsOverviewPressed()
    {
        controlsOptionsMenu.SetActive(false);
        controlsOverviewMenu.SetActive(true);
    }
    
    public void OnReturnToControlsPressed()
    {
        controlsOptionsMenu.SetActive(true);
        controlsOverviewMenu.SetActive(false);
    }

    public void OnBackPressed()
    {
        //Debug.LogWarning("Preferences saved.");
        PlayerPrefs.Save();
        if (!pauseMenu) return;
        
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void HideMenus()
    {
        audioOptionsMenu.SetActive(false);
        graphicsOptionsMenu.SetActive(false);
        controlsOptionsMenu.SetActive(false);
        controlsOverviewMenu.SetActive(false);
    }
    #endregion
}
