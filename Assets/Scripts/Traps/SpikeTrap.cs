using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Урон")]
    public int damage = 1;
    public float damageCooldown = 6f;
    private bool hasDealtDamageThisActivation = false;

    [Header("Время")]
    public float activeDuration = 1f;
    public float cooldownAfterTrigger = 3f;

    [Header("Замедление")]
    public float slowAmount = 0.5f;
    public float slowDuration = 1.5f;

    private Animator animator;
    private Collider2D damageCollider;
    private float nextDamageTime = 0f;

    private bool isActive = false;
    private bool isOnCooldown = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        damageCollider = GetComponent<Collider2D>();

        if (damageCollider != null)
            damageCollider.enabled = false;
        else
            Debug.LogError("damageCollider == null! Добавь Collider2D на шипы!");

        Debug.Log("Шипы инициализированы");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter2D: {other.name}, tag: {other.tag}");

        if (other.CompareTag("Player") && !isActive && !isOnCooldown)
        {
            ActivateTrap();
        }
    }

    void ActivateTrap()
    {
        isActive = true;
        animator.SetTrigger("activate");
        Debug.Log("Шипы активируются (игрок вошёл в зону)");
        Invoke(nameof(EnableDamage), 0.3f);
        Invoke(nameof(DeactivateTrap), activeDuration);
    }

    void DeactivateTrap()
    {
        DisableDamage();
        animator.ResetTrigger("activate");
        // animator.Play("Spikes_Idle");

        isActive = false;
        isOnCooldown = true;
        hasDealtDamageThisActivation = false;

        Debug.Log("Шипы деактивированы");
        Invoke(nameof(ResetCooldown), cooldownAfterTrigger);
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
        Debug.Log("Шипы готовы к новой активации");
    }

    public void EnableDamage()
    {
        if (damageCollider != null)
        {
            damageCollider.enabled = true;
            Debug.Log($"Урон ВКЛЮЧЁН! enabled = {damageCollider.enabled}");
        }
    }

    public void DisableDamage()
    {
        if (damageCollider != null)
        {
            damageCollider.enabled = false;
            Debug.Log($"Урон ВЫКЛЮЧЁН! enabled = {damageCollider.enabled}");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageCollider != null && damageCollider.enabled)
        {
            if (Time.time >= nextDamageTime && !hasDealtDamageThisActivation)
            {
                Debug.Log("НАНОСИМ УРОН ИГРОКУ!");

                PlayerController player = other.GetComponentInParent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    player.ApplySlow(slowAmount, slowDuration);
                    Debug.Log($"Игрок получил урон {damage} и замедление!");
                }
                else
                {
                    Debug.LogError("PlayerController НЕ найден на игроке!");
                }
                nextDamageTime = Time.time + damageCooldown;
                hasDealtDamageThisActivation = true;
            }
        }
    }
}