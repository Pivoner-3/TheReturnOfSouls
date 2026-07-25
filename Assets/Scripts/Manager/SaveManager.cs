using UnityEngine;

public static class SaveManager
{
    private const string SELECTED_HERO = "SelectedHero";
    private const string SAVE_EXISTS = "SaveExists";
    private const string LAST_SCENE = "LastScene";

    public static void SaveSelectedHero(int index)
    {
        PlayerPrefs.SetInt(SELECTED_HERO, index);
        PlayerPrefs.SetInt(SAVE_EXISTS, 1);
        PlayerPrefs.Save();
    }

    public static int GetSelectedHero()
    {
        return PlayerPrefs.GetInt(SELECTED_HERO, 0);
    }

    public static bool HasSave()
    {
        return PlayerPrefs.HasKey(SAVE_EXISTS);
    }

    public static void SaveLastScene(string sceneName)
    {
        PlayerPrefs.SetString(LAST_SCENE, sceneName);
        PlayerPrefs.Save();
    }

    public static string GetLastScene()
    {
        return PlayerPrefs.GetString(LAST_SCENE, "HallwayScene");
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}