using UnityEngine;

public class PickupItem : Interactable
{
    public ItemData itemData;

    public override void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemData);
            promptUI?.SetActive(false);
            Destroy(gameObject);
            Debug.Log($"Подобран предмет: {itemData.itemName}");
        }
    }
}