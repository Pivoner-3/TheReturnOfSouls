using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> items = new List<ItemData>();
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int maxSlots = 20;

    public Image healthBarFill;
    public Text healthText;
    public Image healthBarFill_Inventory;
    public Text healthText_Inventory;
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
            if (inventoryPanel != null)
            {
                bool isOpen = !inventoryPanel.activeSelf;
                inventoryPanel.SetActive(isOpen);
                if (isOpen) OpenInventory();
                else CloseInventory();
            }
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

    // ===== ПОДСЧЁТ КОЛИЧЕСТВА ПРЕДМЕТОВ =====
    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.itemName == itemName)
                count++;
        }
        return count;
    }

    public void RemoveMultipleItems(string itemName, int amount)
    {
        int removed = 0;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].itemName == itemName && removed < amount)
            {
                items.RemoveAt(i);
                removed++;
            }
        }
        RefreshUI();
        SaveInventory();
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

    // ===== СОХРАНЕНИЕ =====
    public void SaveInventory()
    {
        List<string> itemNames = new List<string>();
        foreach (var item in items)
            itemNames.Add(item.itemName);

        string inventoryString = string.Join(",", itemNames);
        PlayerPrefs.SetString("Inventory", inventoryString);
        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        string data = PlayerPrefs.GetString("Inventory", "");
        if (string.IsNullOrEmpty(data)) return;

        string[] names = data.Split(',');
        items.Clear();
        foreach (string name in names)
        {
            ItemData item = Resources.Load<ItemData>("Items/" + name);
            if (item != null)
                items.Add(item);
        }
        RefreshUI();
    }
}