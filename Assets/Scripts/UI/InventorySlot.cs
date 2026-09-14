using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button useButton;
    public Button dropButton;

    private ItemData itemData;

    public void AddItem(ItemData newItem)
    {
        if (newItem == null)
        {
            Debug.LogWarning("❌ newItem == null!");
            return;
        }

        if (icon == null)
        {
            Debug.LogWarning("❌ icon == null! Проверь привязку в префабе InventorySlot.");
            return;
        }

        itemData = newItem;
        icon.sprite = newItem.icon;
        icon.gameObject.SetActive(true);

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(UseItem);

        dropButton.onClick.RemoveAllListeners();
        dropButton.onClick.AddListener(DropItem);
    }

    public void ClearSlot()
    {
        itemData = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        if (itemData == null)
        {
            Debug.LogError("❌ itemData == null в UseItem()! Слот не содержит данных.");
            return;
        }

        string itemName = itemData.itemName;
        int healAmount = itemData.healAmount;
        bool isConsumable = itemData.isConsumable;

        Debug.Log($"Использование предмета: {itemName}, heal={healAmount}, consumable={isConsumable}");
        if (isConsumable && healAmount > 0)
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player == null)
            {
                Debug.LogWarning("❌ Player не найден!");
                return;
            }

            // Лечим
            player.Heal(healAmount);
            Debug.Log($"Использовано зелье! Восстановлено {healAmount} HP");

            // Удаляем из инвентаря
            InventoryManager.Instance.RemoveItem(itemData);
            ClearSlot();
        }
        else
        {
            Debug.Log($"Предмет {itemName} нельзя использовать (не расходник или heal = 0)");
        }
    }

    public void DropItem()
    {
        Debug.Log("1. Начало DropItem");

        if (itemData == null)
        {
            Debug.LogWarning("❌ itemData == null!");
            return;
        }

        string itemName = itemData.itemName;
        Debug.Log($"2. itemData есть: {itemName}");

        Spawner spawner = GetComponent<Spawner>();
        if (spawner == null)
            spawner = GetComponentInChildren<Spawner>();

        if (spawner == null)
        {
            Debug.LogWarning($"❌ В слоте нет компонента Spawner для {itemName}!");
            return;
        }

        Debug.Log("3. Spawner найден, вызываем SpawnDroppedItem()");
        spawner.SpawnDroppedItem();

        Debug.Log("4. Удаляем из инвентаря");
        InventoryManager.Instance.RemoveItem(itemData);

        Debug.Log("5. Очищаем слот");
        ClearSlot();

        Debug.Log($"6. {itemName} выброшен на землю!");
    }
}