using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> items = new List<ItemData>();
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int maxSlots = 8;

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
        // Загружаем инвентарь из сохранения
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
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                RefreshUI();
            }
        }
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

        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in items)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel.transform);
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            if (slotScript != null)
                slotScript.AddItem(item);
        }
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