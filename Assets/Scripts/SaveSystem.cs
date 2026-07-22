using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static SaveData LoadGame()
    {
        if (!SaveExists()) return null;
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }

    public static void DeleteSave()
    {
        if (SaveExists()) File.Delete(SavePath);
    }
}

[System.Serializable]
public class SaveData
{
    public string lastScene = "Prologue";
    public int selectedHero = 0;
    public bool[] freedFriends = new bool[4];
    public int[] foundArtifacts = new int[5];
    public float playTime = 0f;
    public int level = 1;
    public int strength = 5;
    public int agility = 5;
    public int intelligence = 5;
    public int endurance = 5;
}