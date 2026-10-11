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
        if (newItem == null) return;

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
        if (itemData == null) return;

        switch (itemData.itemType)
        {
            case ItemType.Consumable:
                UseConsumable();
                break;

            case ItemType.Weapon:
                EquipWeapon();
                break;

            case ItemType.Armor:
                EquipArmor();
                break;

            case ItemType.Key:
                Debug.Log($"Ключ {itemData.itemName} используется у двери/сундука");
                break;

            case ItemType.Resource:
                Debug.Log($"{itemData.itemName} — ресурс для крафта");
                break;

            default:
                Debug.Log($"Использовать предмет: {itemData.itemName}");
                break;
        }
    }

    private void UseConsumable()
    {
        if (itemData.isConsumable && itemData.healAmount > 0)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                player.Heal(itemData.healAmount);
                InventoryManager.Instance.RemoveItem(itemData);
                ClearSlot();
                Debug.Log($"Использовано зелье! +{itemData.healAmount} HP");
            }
        }
    }

    private void EquipWeapon()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.EquipWeapon(itemData);
            Debug.Log($"Экипировано оружие: {itemData.itemName} (+{itemData.damage} урона)");
        }
    }

    private void EquipArmor()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.EquipArmor(itemData);
            Debug.Log($"Экипирована броня: {itemData.itemName} (+{itemData.defense} защиты)");
        }
    }

    public void DropItem()
    {
        if (itemData == null || itemData.worldPrefab == null) return;

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null) return;

        Vector3 pos = player.transform.position + new Vector3(2f, -0.5f, 0f);
        GameObject dropped = Instantiate(itemData.worldPrefab, pos, Quaternion.identity);
        dropped.GetComponent<PickupItem>().itemData = itemData;

        InventoryManager.Instance.RemoveItem(itemData);
        ClearSlot();
        Debug.Log($"{itemData.itemName} выброшен на землю!");
    }
}