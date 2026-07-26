using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    public CharacterStats stats;
    public int CurrentHealth { get; protected set; }

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
        if (TryGetComponent(out PlayerController player) && player.isBlocking)
        {
            Debug.Log("Урон заблокирован!");
            return;
        }

        CurrentHealth -= amount;
        if (CurrentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        animator.SetTrigger("die");
        Debug.Log("смерть");
    }
}