using UnityEngine;

public class Door : Interactable
{
    public bool isOpen = false;

    public Sprite closedSprite;
    public Sprite openSprite;

    public Collider2D doorCollider; // основной (НЕ триггер)
    public Collider2D triggerCollider; // дополнительный (триггер)

    private SpriteRenderer spriteRenderer;

    new void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateDoorState();
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        UpdateDoorState();
        Debug.Log($"Дверь {(isOpen ? "открыта" : "закрыта")}");
    }

    void UpdateDoorState()
    {
        // Отключаем только основной коллайдер (физический)
        if (doorCollider != null)
            doorCollider.enabled = !isOpen;

        // Триггер всегда активен
        if (triggerCollider != null)
            triggerCollider.enabled = true;

        // Меняем спрайт
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
        }
    }
}