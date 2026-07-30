using UnityEngine;

public class PickupItem : Interactable
{
    public ItemData itemData;

    new void Start()
    {
        if (SaveManager.GetCollectedItems().Contains(itemData.itemName))
        {
            Destroy(gameObject);
        }
    }

    public override void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemData);
            SaveManager.AddCollectedItem(itemData.itemName);
            promptUI?.SetActive(false);
            Destroy(gameObject);
        }
    }
}