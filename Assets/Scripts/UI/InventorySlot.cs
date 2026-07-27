using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public ItemData item;
    public Button useButton;

    public void AddItem(ItemData newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.gameObject.SetActive(true);
        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(UseItem);
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        useButton.onClick.RemoveAllListeners();
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
                ClearSlot();
            }
        }
    }
}