using UnityEngine;

public class EnemyAI : BaseCharacter
{
    public float detectionRange = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private Transform player;
    private float nextAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * stats.speed;

            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                player.GetComponent<IDamageable>()?.TakeDamage(stats.attackPower);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}