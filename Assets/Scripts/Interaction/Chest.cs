using UnityEngine;

public class Chest : Interactable
{
    [Header("Спрайты")]
    public Sprite closedSprite;
    public Sprite openSprite;

    [Header("Предметы")]
    public ItemData[] items;

    public bool isOpened = false;

    private SpriteRenderer spriteRenderer;

    new void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && closedSprite != null)
            spriteRenderer.sprite = closedSprite;
    }

    public override void Interact()
    {
        if (isOpened)
        {
            ChestUI.Instance.CloseChest();
            return;
        }

        isOpened = true;

        if (spriteRenderer != null && openSprite != null)
            spriteRenderer.sprite = openSprite;

        ChestUI.Instance.OpenChest(items, this);
        promptUI?.SetActive(false);

        Debug.Log($"📦 Сундук открыт! {items.Length} предметов");
    }

    public void RemoveItem(ItemData item)
    {
        System.Collections.Generic.List<ItemData> list = new System.Collections.Generic.List<ItemData>(items);
        list.Remove(item);
        items = list.ToArray();
    }

    public void AddItem(ItemData item)
    {
        System.Collections.Generic.List<ItemData> list = new System.Collections.Generic.List<ItemData>(items);
        list.Add(item);
        items = list.ToArray();
    }

    public void Close()
    {
        if (spriteRenderer != null && closedSprite != null)
            spriteRenderer.sprite = closedSprite;

        isOpened = false;
        Debug.Log("Сундук закрыт");
    }
}