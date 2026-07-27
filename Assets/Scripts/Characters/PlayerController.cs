using UnityEngine;

public class PlayerController : BaseCharacter
{
    public bool isBlocking = false;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void EnableBlock()
    {
        isBlocking = true;
        Debug.Log("Блок ВКЛЮЧЁН");
    }
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, stats.maxHealth);
        Debug.Log($"Вылечен на {amount}. Текущее HP: {CurrentHealth}");
    }
    public void DisableBlock()
    {
        isBlocking = false;
        Debug.Log("Блок ВЫКЛЮЧЁН");
    }

    void Update()
    {
        if (isDead) return;

        // Движение
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = movement * stats.speed;

        animator.SetFloat("speed", movement.sqrMagnitude);

        if (moveX < 0) spriteRenderer.flipX = true;
        else if (moveX > 0) spriteRenderer.flipX = false;

        // Атака
        if (Input.GetButtonDown("Fire1"))
            animator.SetTrigger("attack");

        // Блок
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("block");
            isBlocking = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            animator.ResetTrigger("block");
            isBlocking = false;
        }

        // Сохранение
        if (Input.GetKeyDown(KeyCode.F5))
            SavePlayer();
    }

    public override void TakeDamage(int amount)
    {
        if (isDead) return;
        if (isBlocking)
        {
            Debug.Log("Урон заблокирован!");
            return;
        }
        base.TakeDamage(amount);
    }

    public void SavePlayer()
    {
        GameData data = new GameData();
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.playerX = transform.position.x;
        data.playerY = transform.position.y;
        data.playerHealth = CurrentHealth;
        data.selectedHero = SaveManager.GetSelectedHero();
        data.freedFriends = new bool[4];
        data.foundArtifacts = new int[0];
        data.playTime = Time.time;
        data.level = 1;
        data.strength = 5;
        data.agility = 5;
        data.intelligence = 5;
        data.endurance = 5;

        SaveManager.SaveGame(data);
        Debug.Log($"Игрок сохранён: HP={CurrentHealth}, позиция ({data.playerX}, {data.playerY})");
    }

    public void LoadPlayer(GameData data)
    {
        Vector3 pos = transform.position;
        pos.x = data.playerX;
        pos.y = data.playerY;
        transform.position = pos;
        CurrentHealth = data.playerHealth;
        Debug.Log($"Игрок загружен: HP={CurrentHealth}, позиция ({pos.x}, {pos.y})");
    }

    protected override void Die()
    {
        base.Die(); 
        Invoke(nameof(GoToMainMenu), 2f);
    }
    private void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    private void RespawnFromSave()
    {
        if (this == null) return;
        if (!isDead) return;

        if (SaveManager.SaveExists())
        {
            GameData data = SaveManager.LoadGame();

            // Восстанавливаем позицию
            LoadPlayer(data);

            // ВОСКРЕШАЕМ с сохранённым здоровьем
            Revive(data.playerHealth);

            Debug.Log($"Игрок воскрес с HP={data.playerHealth}");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        }
    }
}