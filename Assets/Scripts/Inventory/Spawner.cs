using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject itemPrefab;

    public void SpawnDroppedItem()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("itemPrefab не назначен в Spawner!");
            return;
        }

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;

        Vector3 pos = player.transform.position + new Vector3(2f, -0.5f, 0f);
        Instantiate(itemPrefab, pos, Quaternion.identity);

        Debug.Log($"Создан объект: {itemPrefab.name}");
    }
}