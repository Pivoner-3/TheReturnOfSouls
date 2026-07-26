using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject hitbox;

    public void EnableHitbox()
    {
        if (hitbox != null)
            hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        if (hitbox != null)
            hitbox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(1); // Наносим урон
            Debug.Log("Попал по врагу!");
        }
    }
}