using UnityEngine;

public enum ItemType
{
    Consumable,  // расходник (зелье)
    Weapon,      // оружие (нормальный меч)
    Armor,       // броня
    Key,         // ключ
    Resource,    // ресурс (ржавые мечи)
    Artifact,    // артефакт
    Quest        // квестовый
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    [Header("Основное")]
    public string itemName;
    public Sprite icon;
    public GameObject worldPrefab;
    public ItemType itemType;

    [Header("Расходник")]
    public int healAmount;
    public bool isConsumable;

    [Header("Оружие")]
    public int damage;
    public bool isEquippable;

    [Header("Броня")]
    public int defense;

    [Header("Ключ")]
    public string keyId;

    [Header("Ресурс (для крафта)")]
    public int craftAmount;
}