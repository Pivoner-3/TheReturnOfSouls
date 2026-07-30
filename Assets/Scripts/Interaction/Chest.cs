using UnityEngine;

public class Chest : Interactable
{
    public ItemData itemToGive;
    public bool isOpened = false;
    public Animator animator;

    public override void Interact()
    {
        if (isOpened) return;

        isOpened = true;

        if (animator != null)
            animator.SetTrigger("Open");

        if (itemToGive != null && InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemToGive);
            Debug.Log($"Сундук: получен предмет {itemToGive.itemName}");
        }

        // Если сундук пустой или уже открыт — можно сделать недоступным
        promptUI?.SetActive(false);
    }
}