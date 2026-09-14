using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance;

    public GameObject chestWindow;
    public Transform chestSlotsParent;
    public GameObject slotPrefab;

    private Chest currentChest;
    private ItemData[] chestItems;

    void Awake()
    {
        Instance = this;
        if (chestWindow != null)
            chestWindow.SetActive(false);
    }

    public void OpenChest(ItemData[] items, Chest chest)
    {
        currentChest = chest;
        chestItems = items;
        chestWindow.SetActive(true);

        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in chestSlotsParent)
            Destroy(child.gameObject);

        foreach (var item in chestItems)
        {
            GameObject slot = Instantiate(slotPrefab, chestSlotsParent);
            ChestSlot slotScript = slot.GetComponent<ChestSlot>();
            if (slotScript != null)
                slotScript.Setup(item, this);
        }
    }

    public void TakeItem(ItemData item)
    {
        if (InventoryManager.Instance == null) return;

        // Добавляем в инвентарь игрока
        InventoryManager.Instance.AddItem(item);

        // Удаляем из сундука
        currentChest.RemoveItem(item);
        chestItems = currentChest.items;

        RefreshUI();
        Debug.Log($"Взято из сундука: {item.itemName}");
    }

    public void ReturnItem(ItemData item)
    {
        // Возвращаем предмет обратно в сундук
        currentChest.AddItem(item);
        chestItems = currentChest.items;

        // Удаляем из инвентаря игрока
        InventoryManager.Instance.RemoveItem(item);

        RefreshUI();
        Debug.Log($"Возвращено в сундук: {item.itemName}");
    }

    public void CloseChest()
    {
        chestWindow.SetActive(false);
        currentChest?.Close();
        currentChest = null;
    }
}