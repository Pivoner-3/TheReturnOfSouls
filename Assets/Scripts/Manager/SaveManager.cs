using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string SavePath => Application.persistentDataPath + "/save.json";
    private const string SELECTED_HERO = "SelectedHero";
    private const string LAST_SCENE = "LastScene";

    // ===== —Œ’–¿Õ≈Õ»≈ =====
    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"»„‡ ÒÓı‡ÌÂÌ‡: {SavePath}");
    }

    // ===== «¿√–”« ¿ =====
    public static GameData LoadGame()
    {
        if (!SaveExists()) return null;
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameData>(json);
    }

    // ===== œ–Œ¬≈– ¿ =====
    public static bool SaveExists() => File.Exists(SavePath);

    // ===== ”ƒ¿À≈Õ»≈ =====
    public static void DeleteSave()
    {
        if (SaveExists()) File.Delete(SavePath);
    }

    // ===== ¬€¡Œ– √≈–Œﬂ =====
    public static void SaveSelectedHero(int index)
    {
        PlayerPrefs.SetInt(SELECTED_HERO, index);
        PlayerPrefs.Save();
    }

    public static int GetSelectedHero()
    {
        return PlayerPrefs.GetInt(SELECTED_HERO, 0);
    }

    // ===== œŒ—À≈ƒÕﬂﬂ —÷≈Õ¿ =====
    public static void SaveLastScene(string sceneName)
    {
        PlayerPrefs.SetString(LAST_SCENE, sceneName);
        PlayerPrefs.Save();
    }

    public static string GetLastScene()
    {
        return PlayerPrefs.GetString(LAST_SCENE, "HallwayScene");
    }
    public static void AddKilledEnemy(string enemyId)
    {
        List<string> killed = GetKilledEnemies();
        if (!killed.Contains(enemyId)) killed.Add(enemyId);
        PlayerPrefs.SetString("KilledEnemies", string.Join(",", killed));
    }
    public static void ClearKilledEnemies()
    {
        PlayerPrefs.DeleteKey("KilledEnemies");
        PlayerPrefs.Save();
    }
    public static List<string> GetKilledEnemies()
    {
        string data = PlayerPrefs.GetString("KilledEnemies", "");
        if (string.IsNullOrEmpty(data)) return new List<string>();
        return new List<string>(data.Split(','));
    }
}

[System.Serializable]
public class GameData
{
    public string sceneName;
    public float playerX;
    public float playerY;
    public int playerHealth;
    public int selectedHero;
    public bool[] freedFriends;
    public int[] foundArtifacts;
    public bool[] killedEnemies;
    public float playTime;
    public int level;
    public int strength;
    public int agility;
    public int intelligence;
    public int endurance;
}