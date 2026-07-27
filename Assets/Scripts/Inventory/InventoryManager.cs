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
        Instance = this;
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
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
        }
    }

    void RefreshUI()
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
}