using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    void Start()
    {
        HeroData selected = HeroManager.Instance.GetSelectedHero();
        Instantiate(selected.prefab, transform.position, Quaternion.identity);
    }
}