using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNewGame()
    {
        SaveManager.DeleteSave();
        PlayerPrefs.DeleteKey("Inventory");
        SaveManager.ClearCollectedItems();
        SaveManager.ClearKilledEnemies();
        LoadScene("ChoosingScene");
    }

    public void ContinueGame()
    {
        if (SaveManager.SaveExists())
        {
            GameData data = SaveManager.LoadGame();
            LoadScene(data.sceneName);
        }
    }

    public void LoadScene(string sceneName)
    {
        SaveManager.SaveLastScene(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}