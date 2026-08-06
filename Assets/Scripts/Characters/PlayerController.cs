using UnityEngine;

public class PlayerController : BaseCharacter
{
    private float originalSpeed;
    private Coroutine slowCoroutine;
    public bool isBlocking = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        originalSpeed = stats.speed;
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        // Если уже замедлен — сбрасываем
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        // Применяем замедление
        stats.speed = originalSpeed * (1f - slowAmount);
        Debug.Log($"Игрок замедлен! Скорость: {stats.speed}");

        // Запускаем восстановление через время
        slowCoroutine = StartCoroutine(RemoveSlowAfter(duration));

    }

    private System.Collections.IEnumerator RemoveSlowAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        stats.speed = originalSpeed;
        Debug.Log($"Скорость восстановлена: {stats.speed}");
        slowCoroutine = null;
    }

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
        FlashGreen();
        Debug.Log($"Вылечен на {amount}. Текущее HP: {CurrentHealth}");

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.UpdateHealthUI();
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
        FlashRed();
        base.TakeDamage(amount);

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.UpdateHealthUI();
    }
    public void SavePlayer()
    {
        GameData data = new GameData();

        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.playerX = transform.position.x;
        data.playerY = transform.position.y;
        data.playerHealth = CurrentHealth;
        data.selectedHero = SaveManager.GetSelectedHero();

        data.killedEnemies = new bool[0];

        if (InventoryManager.Instance != null)
        {
            string[] itemNames = new string[InventoryManager.Instance.items.Count];
            for (int i = 0; i < InventoryManager.Instance.items.Count; i++)
            {
                itemNames[i] = InventoryManager.Instance.items[i].itemName;
            }
            SaveManager.SaveInventory(itemNames);
        }

        data.freedFriends = new bool[4];
        data.foundArtifacts = new int[0];
        data.playTime = Time.time;
        data.level = 1;
        data.strength = 5;
        data.agility = 5;
        data.intelligence = 5;
        data.endurance = 5;

        SaveManager.SaveGame(data);
        Debug.Log("Игрок сохранён!");
    }

    public void LoadPlayer(GameData data)
    {
        Vector3 pos = transform.position;
        pos.x = data.playerX;
        pos.y = data.playerY;
        transform.position = pos;
        CurrentHealth = data.playerHealth;

        if (data.inventoryItems != null && InventoryManager.Instance != null)
        {
            foreach (string itemName in data.inventoryItems)
            {
                ItemData item = Resources.Load<ItemData>($"Items/{itemName}");
                if (item != null)
                    InventoryManager.Instance.items.Add(item);
            }
            InventoryManager.Instance.RefreshUI();
        }

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
    public void FlashRed()
    {
        StartCoroutine(FlashRedCoroutine());
    }

    private System.Collections.IEnumerator FlashRedCoroutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    public void FlashGreen()
    {
        StartCoroutine(FlashGreenCoroutine());
    }

    private System.Collections.IEnumerator FlashGreenCoroutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        sr.color = Color.green;
        yield return new WaitForSeconds(0.15f);
        sr.color = Color.white;
    }
}