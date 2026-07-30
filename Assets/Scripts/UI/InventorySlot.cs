using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public ItemData item;
    public Button useButton;
    public Button dropButton;

    public void AddItem(ItemData newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.gameObject.SetActive(true);

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(UseItem);

        dropButton.onClick.RemoveAllListeners();
        dropButton.onClick.AddListener(DropItem);
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        useButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();
    }

    public void UseItem()
    {
        if (item == null) return;

        if (item.isConsumable && item.healAmount > 0)
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                player.Heal(item.healAmount);
                InventoryManager.Instance.RemoveItem(item);
                ClearSlot();
            }
        }
    }

    public void DropItem()
    {
        if (item == null) return;

        ItemData itemToDrop = item;

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;

        // === ГЛАВНОЕ: УДАЛЯЕМ ИЗ COLLECTEDITEMS ===
        SaveManager.RemoveCollectedItem(itemToDrop.itemName);

        // Удаляем из инвентаря
        InventoryManager.Instance.RemoveItem(itemToDrop);
        ClearSlot();

        // Создаём предмет на земле
        Vector3 dropPosition = player.transform.position + new Vector3(3f, -0.5f, 0f);
        GameObject droppedObject = new GameObject(itemToDrop.itemName);
        droppedObject.transform.position = dropPosition;

        SpriteRenderer sr = droppedObject.AddComponent<SpriteRenderer>();
        sr.sprite = itemToDrop.icon;
        sr.sortingOrder = 5;

        BoxCollider2D collider = droppedObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1f, 1f);

        Rigidbody2D rb = droppedObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        PickupItem pickup = droppedObject.AddComponent<PickupItem>();
        pickup.itemData = itemToDrop;
        pickup.promptUI = GameObject.Find("InteractionPrompt");
        pickup.enabled = true;

        Debug.Log($"Предмет {itemToDrop.itemName} выброшен на землю!");
    }

    private System.Collections.IEnumerator EnablePickupAfterDelay(PickupItem pickup)
    {
        yield return new WaitForSeconds(0.3f);
        pickup.enabled = true;
        Debug.Log("PickupItem снова активен!");
    }
}