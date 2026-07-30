using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string SavePath => Application.persistentDataPath + "/save.json";
    private const string SELECTED_HERO = "SelectedHero";
    private const string LAST_SCENE = "LastScene";

    // ===== СОХРАНЕНИЕ =====
    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Игра сохранена: {SavePath}");
    }

    // ===== ЗАГРУЗКА =====
    public static GameData LoadGame()
    {
        if (!SaveExists()) return null;
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameData>(json);
    }

    // ===== ПРОВЕРКА =====
    public static bool SaveExists() => File.Exists(SavePath);

    // ===== УДАЛЕНИЕ =====
    public static void DeleteSave()
    {
        if (SaveExists()) File.Delete(SavePath);
        PlayerPrefs.DeleteKey("Inventory");
        PlayerPrefs.DeleteKey("KilledEnemies");
    }

    // ===== ВЫБОР ГЕРОЯ =====
    public static void SaveSelectedHero(int index)
    {
        PlayerPrefs.SetInt(SELECTED_HERO, index);
        PlayerPrefs.Save();
    }

    public static int GetSelectedHero()
    {
        return PlayerPrefs.GetInt(SELECTED_HERO, 0);
    }

    // ===== ПОСЛЕДНЯЯ СЦЕНА =====
    public static void SaveLastScene(string sceneName)
    {
        PlayerPrefs.SetString(LAST_SCENE, sceneName);
        PlayerPrefs.Save();
    }

    public static string GetLastScene()
    {
        return PlayerPrefs.GetString(LAST_SCENE, "HallwayScene");
    }

    // ===== СОХРАНЕНИЕ ИНВЕНТАРЯ (НОВОЕ) =====
    public static void SaveInventory(string[] itemNames)
    {
        string inventoryString = string.Join(",", itemNames);
        PlayerPrefs.SetString("Inventory", inventoryString);
        PlayerPrefs.Save();
    }

    public static string[] LoadInventory()
    {
        string data = PlayerPrefs.GetString("Inventory", "");
        if (string.IsNullOrEmpty(data)) return new string[0];
        return data.Split(',');
    }
    // ===== УБИТЫЕ МОНСТРЫ (ВОЗВРАЩЕНЫ) =====
    public static void AddKilledEnemy(string enemyId)
    {
        List<string> killed = GetKilledEnemies();
        if (!killed.Contains(enemyId)) killed.Add(enemyId);
        PlayerPrefs.SetString("KilledEnemies", string.Join(",", killed));
        PlayerPrefs.Save();
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

    public static void AddCollectedItem(string itemName)
    {
        List<string> collected = GetCollectedItems();
        if (!collected.Contains(itemName)) collected.Add(itemName);
        PlayerPrefs.SetString("CollectedItems", string.Join(",", collected));
        PlayerPrefs.Save();
    }

    public static void RemoveCollectedItem(string itemName)
    {
        List<string> collected = GetCollectedItems();
        if (collected.Contains(itemName))
        {
            collected.Remove(itemName);
            PlayerPrefs.SetString("CollectedItems", string.Join(",", collected));
            PlayerPrefs.Save();
        }
    }

    public static List<string> GetCollectedItems()
    {
        string data = PlayerPrefs.GetString("CollectedItems", "");
        if (string.IsNullOrEmpty(data)) return new List<string>();
        return new List<string>(data.Split(','));
    }

    public static void ClearCollectedItems()
    {
        PlayerPrefs.DeleteKey("CollectedItems");
        PlayerPrefs.Save();
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
    public string[] inventoryItems;
    public string[] collectedItems;
    public float playTime;
    public int level;
    public int strength;
    public int agility;
    public int intelligence;
    public int endurance;
}