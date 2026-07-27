using UnityEngine;

public class EnemyAI : BaseCharacter
{
    public float detectionRange = 4f;
    public float attackRange = 1.5f;
    public float patrolSpeed = 1f;
    public float chaseSpeed = 3f;
    public float patrolDistance = 3f;

    private Transform player;
    private float nextAttackTime;
    private float leftBound;
    private float rightBound;
    private bool movingRight = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        leftBound = transform.position.x - patrolDistance;
        rightBound = transform.position.x + patrolDistance;

        if (SaveManager.GetKilledEnemies().Contains(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (CurrentHealth <= 0) return;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        // Проверяем, жив ли игрок
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null && pc.isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Chase();

            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                Debug.Log("Враг атакует игрока!");
                player.GetComponent<IDamageable>()?.TakeDamage(stats.attackPower);
                nextAttackTime = Time.time + stats.attackCooldown;
            }
            return;
        }

        Patrol();
    }

    void Chase()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
        Flip(direction.x < 0);
    }

    void Patrol()
    {
        float speed = movingRight ? patrolSpeed : -patrolSpeed;
        rb.linearVelocity = new Vector2(speed, 0);

        if (transform.position.x >= rightBound) movingRight = false;
        else if (transform.position.x <= leftBound) movingRight = true;

        Flip(movingRight);
    }

    void Flip(bool left)
    {
        transform.localScale = new Vector3(left ? -1 : 1, 1, 1);
    }

    protected override void Die()
    {
        // Запускаем базовую логику (анимация, отключение управления)
        base.Die();

        // ⚠️ ВРЕМЕННО: просто логируем смерть
        Debug.Log($"Враг {gameObject.name} умер!");
        SaveManager.AddKilledEnemy(gameObject.name); // вызовешь позже
    }
}