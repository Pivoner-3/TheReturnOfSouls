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

    public Image healthBarFill;      // HUD-полоска здоровья
    public Text healthText;          // HUD-текст здоровья

    public Image healthBarFill_Inventory;   // полоска в инвентаре
    public Text healthText_Inventory;       // текст в инвентаре

    public Image strengthBarFill;
    public Text strengthText;
    public Image agilityBarFill;
    public Text agilityText;

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
        string[] savedItems = SaveManager.LoadInventory();
        if (savedItems.Length > 0)
        {
            items.Clear();
            foreach (string itemName in savedItems)
            {
                ItemData foundItem = FindItemByName(itemName);
                if (foundItem != null)
                    items.Add(foundItem);
            }
            RefreshUI();
            Debug.Log($"Инвентарь загружен: {items.Count} предметов");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel != null)
            {
                bool isOpen = !inventoryPanel.activeSelf;
                inventoryPanel.SetActive(isOpen);

                if (isOpen)
                {
                    OpenInventory();
                }
                else
                {
                    CloseInventory();
                }
            }
        }

        if (inventoryPanel != null && inventoryPanel.activeSelf)
        {
            UpdateStatsUI();
        }
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        RefreshUI();
        UpdateHealthUI();
        UpdateStatsUI();

        Transform heroPanel = inventoryPanel.transform.Find("HeroPanel");
        if (heroPanel != null && !heroPanel.gameObject.activeSelf)
        {
            heroPanel.gameObject.SetActive(true);
            Debug.Log("HeroPanel принудительно активирован");
        }

        if (hudPanel != null)
            hudPanel.SetActive(false);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        if (hudPanel != null)
            hudPanel.SetActive(true);
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
        Debug.Log($"Подобран предмет: {item.itemName}");
    }

    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            RefreshUI();
            Debug.Log($"Предмет {item.itemName} удалён из инвентаря");
        }
    }

    public void RefreshUI()
    {
        if (inventoryPanel == null || slotPrefab == null) return;

        // Удаляем только слоты (а не всю панель)
        Transform slotGrid = inventoryPanel.transform.Find("SlotGridContainer");
        if (slotGrid == null) return;

        foreach (Transform child in slotGrid)
        {
            Destroy(child.gameObject);
        }

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
        if (player == null)
        {
            Debug.LogWarning("PlayerController не найден!");
            return;
        }

        int current = player.CurrentHealth;
        int max = player.stats.maxHealth;
        float fill = (float)current / max;

        // HUD
        if (healthBarFill != null)
            healthBarFill.fillAmount = fill;
        if (healthText != null)
            healthText.text = $"{current} / {max}";

        // Инвентарь
        if (healthBarFill_Inventory != null)
        {
            healthBarFill_Inventory.fillAmount = fill;
            Debug.Log($"Инвентарь: fill = {healthBarFill_Inventory.fillAmount}");
        }
        if (healthText_Inventory != null)
        {
            healthText_Inventory.text = $"{current} / {max}";
            Debug.Log($"Инвентарь: текст = {healthText_Inventory.text}");
        }
    }

    public void UpdateStatsUI()
    {
        if (strengthBarFill != null)
            strengthBarFill.fillAmount = 0.8f;
        if (strengthText != null)
            strengthText.text = "8";

        if (agilityBarFill != null)
            agilityBarFill.fillAmount = 0.6f;
        if (agilityText != null)
            agilityText.text = "6";
    }

    private ItemData FindItemByName(string name)
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
        foreach (var item in allItems)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }
}