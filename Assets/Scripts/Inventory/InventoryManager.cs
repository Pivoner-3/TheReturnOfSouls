using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> items = new List<ItemData>();
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int maxSlots = 8;

    // Здоровье
    public Image healthBarFill;
    public Text healthText;
    public Image healthBarFill_Inventory;
    public Text healthText_Inventory;

    // Статы
    public Image strengthBarFill;
    public Text strengthText;
    public Image agilityBarFill;
    public Text agilityText;

    // HUD
    public GameObject hudPanel;

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

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    void Start()
    {
        LoadInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isOpen = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isOpen);
            if (isOpen) OpenInventory();
            else CloseInventory();
        }
    }

    public void OpenInventory()
    {
        RefreshUI();
        UpdateHealthUI();
        if (hudPanel != null) hudPanel.SetActive(false);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(true);
    }

    public void AddItem(ItemData item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("Инвентарь полон!");
            return;
        }
        items.Add(item);
        RefreshUI();
        SaveInventory();
        Debug.Log($"✅ Добавлен предмет: {item.itemName}");
    }

    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            RefreshUI();
            SaveInventory();
            Debug.Log($"❌ Удалён предмет: {item.itemName}");
        }
    }

    public void RefreshUI()
    {
        if (inventoryPanel == null || slotPrefab == null) return;

        Transform slotGrid = inventoryPanel.transform.Find("SlotGridContainer");
        if (slotGrid == null) return;

        foreach (Transform child in slotGrid)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject slot = Instantiate(slotPrefab, slotGrid);
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            if (slotScript != null)
                slotScript.AddItem(item);
        }
    }

    public void UpdateHealthUI()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;

        int current = player.CurrentHealth;
        int max = player.stats.maxHealth;
        float fill = (float)current / max;

        if (healthBarFill != null) healthBarFill.fillAmount = fill;
        if (healthText != null) healthText.text = $"{current} / {max}";
        if (healthBarFill_Inventory != null) healthBarFill_Inventory.fillAmount = fill;
        if (healthText_Inventory != null) healthText_Inventory.text = $"{current} / {max}";
    }

    public void UpdateStatsUI()
    {
        if (strengthBarFill != null) strengthBarFill.fillAmount = 0.8f;
        if (strengthText != null) strengthText.text = "8";
        if (agilityBarFill != null) agilityBarFill.fillAmount = 0.6f;
        if (agilityText != null) agilityText.text = "6";
    }

    // ===== СОХРАНЕНИЕ =====
    public void SaveInventory()
    {
        List<string> itemNames = new List<string>();
        foreach (var item in items)
        {
            itemNames.Add(item.itemName);
        }

        string inventoryString = string.Join(",", itemNames);
        PlayerPrefs.SetString("Inventory", inventoryString);
        PlayerPrefs.Save();
        Debug.Log($"💾 Инвентарь сохранён: {inventoryString} (предметов: {items.Count})");
    }

    private void LoadInventory()
    {
        string data = PlayerPrefs.GetString("Inventory", "");
        Debug.Log($"📂 Загружаем инвентарь: data = '{data}'");

        if (string.IsNullOrEmpty(data))
        {
            Debug.Log("📂 Инвентарь пуст (нет сохранения)");
            return;
        }

        string[] names = data.Split(',');
        items.Clear();
        foreach (string name in names)
        {
            Debug.Log($"🔍 Ищем ItemData: Items/{name}");
            ItemData item = Resources.Load<ItemData>("Items/" + name);
            if (item != null)
            {
                items.Add(item);
                Debug.Log($"✅ Загружен предмет: {name}");
            }
            else
            {
                Debug.LogWarning($"❌ Не найден ItemData: Items/{name}");
            }
        }
        RefreshUI();
        Debug.Log($"📂 Инвентарь загружен: {items.Count} предметов");
    }
}