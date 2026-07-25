using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Game/HeroData")]
public class HeroData : ScriptableObject
{
    public string heroName;
    public GameObject prefab;
    public CharacterStats stats;
    public Sprite portrait;
}