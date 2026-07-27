using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    public CharacterStats stats;
    public int CurrentHealth { get; protected set; }
    public bool isDead = false;

    protected Animator animator;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentHealth = stats.maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead) return;
        CurrentHealth -= amount;
        if (CurrentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null) animator.SetTrigger("die");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        this.enabled = false;
    }

    // ===== ВОСКРЕШЕНИЕ С ВОССТАНОВЛЕНИЕМ ЗДОРОВЬЯ ИЗ СОХРАНЕНИЯ =====
    public virtual void Revive(int healthFromSave)
    {
        isDead = false;
        CurrentHealth = healthFromSave; // ← здоровье из сохранения

        this.enabled = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.ResetTrigger("die");
            animator.Play("Idle", 0, 0f);
        }
    }
}