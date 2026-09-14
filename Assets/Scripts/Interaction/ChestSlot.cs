using UnityEngine;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour
{
    public Image icon;
    public Button takeButton;
    public Button returnButton;

    private ItemData item;
    private ChestUI chestUI;

    public void Setup(ItemData newItem, ChestUI ui)
    {
        item = newItem;
        chestUI = ui;

        icon.sprite = newItem.icon;
        icon.gameObject.SetActive(true);

        takeButton.onClick.RemoveAllListeners();
        takeButton.onClick.AddListener(() => chestUI.TakeItem(item));
    }
}