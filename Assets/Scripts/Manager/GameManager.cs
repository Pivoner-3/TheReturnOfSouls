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

    public void LoadScene(string sceneName)
    {
        SaveManager.SaveLastScene(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void StartNewGame()
    {
        SaveManager.DeleteSave();
        LoadScene("ChoosingScene");
    }

    public void ContinueGame()
    {
        if (SaveManager.HasSave())
        {
            LoadScene(SaveManager.GetLastScene());
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}