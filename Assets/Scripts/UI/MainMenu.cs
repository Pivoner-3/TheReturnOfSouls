using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button newGameButton;
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject achievementsPanel;

    [Header("Settings")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle fullscreenToggle;

    void Start()
    {
        continueButton.interactable = SaveManager.HasSave();

        newGameButton.onClick.AddListener(() => GameManager.Instance.StartNewGame());
        continueButton.onClick.AddListener(() => GameManager.Instance.ContinueGame());
        quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());

        settingsButton.onClick.AddListener(() => settingsPanel.SetActive(true));
        // ... кнопка закрытия панели и т.д.
    }
}