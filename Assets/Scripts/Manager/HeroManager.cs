using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public static HeroManager Instance;

    public HeroData[] heroes;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public HeroData GetSelectedHero()
    {
        int index = SaveManager.GetSelectedHero();
        return heroes[index];
    }
}