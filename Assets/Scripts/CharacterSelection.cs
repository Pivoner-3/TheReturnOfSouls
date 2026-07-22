using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [Header("Портреты героев")]
    public Button swaronButton;
    public Button gaerButton;
    public Button kreinButton;
    public Button ivanisButton;

    [Header("Информационная панель")]
    public Text heroNameText;
    public Text heroDescriptionText;
    public Text heroStatsText;

    [Header("Кнопки управления")]
    public Button selectButton;
    public Button backButton;

    private int selectedHeroIndex = 0;

    private string[] heroNames = { "Сварон", "Гаер", "Крейн", "Иванис" };
    private string[] heroDescriptions = {
        "Молодой мечник, бывший солдат Бургунда. Справедливый и благородный рыцарь.",
        "Искусный убийца и вор из клана ассасинов. Циничный, но верный товарищ.",
        "Маг из Храмовии, борец с культом Ревельдеризма. Ищет истину.",
        "Бывший лучник армии Дачлэнда. Спокойный, молчаливый, слегка заносчивый."
    };
    private string[] heroStats = {
        "Сила: 8  Ловкость: 6  Интеллект: 4  Выносливость: 7",
        "Сила: 5  Ловкость: 9  Интеллект: 3  Выносливость: 6",
        "Сила: 4  Ловкость: 5  Интеллект: 9  Выносливость: 5",
        "Сила: 6  Ловкость: 8  Интеллект: 4  Выносливость: 6"
    };

    private void Start()
    {
        swaronButton.onClick.AddListener(() => SelectHero(0));
        gaerButton.onClick.AddListener(() => SelectHero(1));
        kreinButton.onClick.AddListener(() => SelectHero(2));
        ivanisButton.onClick.AddListener(() => SelectHero(3));

        selectButton.onClick.AddListener(OnSelectHero);
        backButton.onClick.AddListener(OnBack);

        SelectHero(0);
    }

    public void SelectHero(int index)
    {
        selectedHeroIndex = index;

        heroNameText.text = heroNames[index];
        heroDescriptionText.text = heroDescriptions[index];
        heroStatsText.text = heroStats[index];
    }

    private Button GetButtonByIndex(int index)
    {
        switch (index)
        {
            case 0: return swaronButton;
            case 1: return gaerButton;
            case 2: return kreinButton;
            case 3: return ivanisButton;
            default: return swaronButton;
        }
    }

    public void OnSelectHero()
    {
        PlayerPrefs.SetInt("SelectedHero", selectedHeroIndex);
        PlayerPrefs.Save();
        Debug.Log($"Выбран герой: {heroNames[selectedHeroIndex]}");
        SceneManager.LoadScene("HallwayScene");
    }

    public void OnBack()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}