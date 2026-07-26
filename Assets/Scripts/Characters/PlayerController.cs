using UnityEngine;

public class PlayerController : BaseCharacter
{
    private SpriteRenderer spriteRenderer;
    public bool isBlocking = false;

    public void EnableBlock()
    {
        isBlocking = true;
        Debug.Log("Блок ВКЛЮЧЁН");
    }

    public void DisableBlock()
    {
        isBlocking = false;
        Debug.Log("Блок ВЫКЛЮЧЁН");
    }

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = movement * stats.speed;

        // Анимация
        animator.SetFloat("speed", movement.sqrMagnitude);

        // Поворот
        if (moveX < 0) spriteRenderer.flipX = true;
        else if (moveX > 0) spriteRenderer.flipX = false;

        // Атака
        if (Input.GetButtonDown("Fire1"))
            animator.SetTrigger("attack");
        // Блок
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("block");
        }
    }
}