using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(itemData);
                Destroy(gameObject);
            }
        }
    }
}