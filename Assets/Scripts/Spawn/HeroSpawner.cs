using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    void Start()
    {
        HeroData hero = HeroManager.Instance.GetSelectedHero();
        GameObject player = Instantiate(hero.prefab, transform.position, Quaternion.identity);
        PlayerController pc = player.GetComponent<PlayerController>();

        if (SaveManager.SaveExists())
        {
            GameData data = SaveManager.LoadGame();
            pc.LoadPlayer(data); 
            Debug.Log($"Игрок загружен на позицию: ({data.playerX}, {data.playerY})");
        }
    }
}