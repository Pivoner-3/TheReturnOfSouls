using UnityEngine;

public class PlayerController : BaseCharacter
{
    public float moveSpeed = 5f;
    public Transform attackPoint;
    private SpriteRenderer spriteRenderer;

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
    }

    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, 1f);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable target))
                target.TakeDamage(stats.attackPower);
        }
    }
}