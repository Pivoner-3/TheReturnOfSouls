using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header(" ÌÓÔÍË „Î‡‚ÌÓ„Ó ÏÂÌ˛")]
    public Button startButton;
    public Button continueButton;
    public Button settingsButton;
    public Button achievementsButton;
    public Button quitButton;

    [Header("œ‡ÌÂÎË")]
    public GameObject settingsPanel;
    public GameObject achievementsPanel;

    [Header("Õ‡ÒÚÓÈÍË Á‚ÛÍ‡")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Õ‡ÒÚÓÈÍË ˝Í‡Ì‡")]
    public Toggle fullscreenToggle;
    void Start()
    {
        bool hasSave = SaveSystem.SaveExists();
        continueButton.interactable = hasSave;

        startButton.onClick.AddListener(OnStartGame);
        continueButton.onClick.AddListener(OnContinueGame);
        settingsButton.onClick.AddListener(OnOpenSettings);
        achievementsButton.onClick.AddListener(OnOpenAchievements);
        quitButton.onClick.AddListener(OnQuitGame);

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (achievementsPanel != null) achievementsPanel.SetActive(false);
        LoadSettings();
    }

    public void OnStartGame()
    {
        Debug.Log("ÕŒ¬¿ﬂ »√–¿");
    }

    public void OnContinueGame()
    {
        Debug.Log("œ–ŒƒŒÀ∆»“‹");
        if (SaveSystem.SaveExists())
        {
            SaveData data = SaveSystem.LoadGame();
            if (data != null)
            {
                Debug.Log($"«‡„ÛÊÂÌ‡ ÒˆÂÌ‡: {data.lastScene}");
            }
        }
    }

    public void OnOpenSettings()
    {
        Debug.Log("Œ“ –€“‹ Õ¿—“–Œ… »");
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        Debug.Log("«¿ –€“‹ Õ¿—“–Œ… »");
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            SaveSettings();
        }
    }

    public void OnOpenAchievements()
    {
        Debug.Log("Œ“ –€“‹ ƒŒ—“»∆≈Õ»ﬂ");
        if (achievementsPanel != null) achievementsPanel.SetActive(true);
    }

    public void OnCloseAchievements()
    {
        Debug.Log("«¿ –€“‹ ƒŒ—“»∆≈Õ»ﬂ");
        if (achievementsPanel != null) achievementsPanel.SetActive(false);
    }

    public void OnQuitGame()
    {
        Debug.Log("¬€’Œƒ »« »√–€");
        Application.Quit();
    }


    public void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    public void OnMusicVolumeChanged(float value)
    {
        GameObject music = GameObject.FindGameObjectWithTag("Music");
        if (music != null)
        {
            AudioSource source = music.GetComponent<AudioSource>();
            if (source != null) source.volume = value;
        }
    }

    public void OnSFXVolumeChanged(float value)
    {
        GameObject[] sfxSources = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject go in sfxSources)
        {
            AudioSource source = go.GetComponent<AudioSource>();
            if (source != null) source.volume = value;
        }
    }

    public void OnFullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void LoadSettings()
    {
        if (masterVolumeSlider != null)
        {
            float saved = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
            masterVolumeSlider.value = saved;
            AudioListener.volume = saved;
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        }

        if (fullscreenToggle != null)
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            fullscreenToggle.isOn = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }
    }

    private void SaveSettings()
    {
        if (masterVolumeSlider != null)
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);

        if (musicVolumeSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

        if (sfxVolumeSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

        if (fullscreenToggle != null)
            PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
    }
    public void CloseSettingsPanel()
    {
        OnCloseSettings();
    }

    public void CloseAchievementsPanel()
    {
        OnCloseAchievements();
    }
}
