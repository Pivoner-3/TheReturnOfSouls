using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Кнопки главного меню")]
    public Button newGameButton;
    public Button continueButton;
    public Button settingsButton;
    public Button achievementsButton;
    public Button quitButton;

    [Header("Панели")]
    public GameObject settingsPanel;
    public GameObject achievementsPanel;

    [Header("Настройки звука")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Настройки экрана")]
    public Toggle fullscreenToggle;

    void Start()
    {
        // Кнопка "Продолжить" активна только если есть сохранение
        continueButton.interactable = SaveManager.SaveExists();

        // Подписка кнопок
        newGameButton.onClick.AddListener(() => GameManager.Instance.StartNewGame());
        continueButton.onClick.AddListener(() => GameManager.Instance.ContinueGame());
        settingsButton.onClick.AddListener(OpenSettings);
        achievementsButton.onClick.AddListener(OpenAchievements);
        quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());

        // Скрываем панели при старте
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (achievementsPanel != null) achievementsPanel.SetActive(false);

        // Загружаем настройки
        LoadSettings();
    }

    // ===== ОТКРЫТИЕ/ЗАКРЫТИЕ ПАНЕЛЕЙ =====
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            SaveSettings();
        }
    }

    public void OpenAchievements()
    {
        if (achievementsPanel != null) achievementsPanel.SetActive(true);
    }

    public void CloseAchievements()
    {
        if (achievementsPanel != null) achievementsPanel.SetActive(false);
    }

    // ===== НАСТРОЙКИ ЗВУКА =====
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

    // ===== НАСТРОЙКИ ЭКРАНА =====
    public void OnFullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // ===== СОХРАНЕНИЕ НАСТРОЕК =====
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

    // ===== МЕТОДЫ ДЛЯ КНОПОК "НАЗАД" =====
    public void CloseSettingsPanel()
    {
        CloseSettings();
    }

    public void CloseAchievementsPanel()
    {
        CloseAchievements();
    }
}