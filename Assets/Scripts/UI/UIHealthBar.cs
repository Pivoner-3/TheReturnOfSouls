using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [Header("Компоненты UI")]
    public Image healthFill;
    public Text healthText;

    [Header("Настройки")]
    public float smoothSpeed = 5f;
    public Color healthyColor = Color.green;
    public Color damagedColor = Color.yellow;
    public Color criticalColor = Color.red;

    private PlayerController player;
    private float targetFill;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogWarning("UIHealthBar: Player не найден!");
            return;
        }

        targetFill = 1f;
    }

    void Update()
    {
        if (player == null) return;

        int current = player.CurrentHealth;
        int max = player.stats.maxHealth;
        targetFill = (float)current / max;

        // 1. Плавное обновление полоски
        if (healthFill != null)
        {
            healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, targetFill, Time.deltaTime * smoothSpeed);
        }

        // 2. Цвет полоски в зависимости от здоровья
        if (healthFill != null)
        {
            if (targetFill > 0.6f)
                healthFill.color = healthyColor;
            else if (targetFill > 0.3f)
                healthFill.color = damagedColor;
            else
                healthFill.color = criticalColor;
        }

        // 3. Текст
        if (healthText != null)
        {
            healthText.text = $"{current} / {max}";
        }
    }
}