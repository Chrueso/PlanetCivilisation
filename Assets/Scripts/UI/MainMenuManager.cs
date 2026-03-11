using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // other scripts can read this to know which faction was picked
    public static FactionType SelectedFaction = FactionType.Human;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject factionSelectOverlay;

    [Header("Faction Select Buttons")]
    [SerializeField] private Button humanButton;
    [SerializeField] private Button demiHumanButton;
    [SerializeField] private Button constructButton;
    [SerializeField] private Button backButton;

    private void Start()
    {
        // wire start button by name
        foreach (var btn in GetComponentsInChildren<Button>(true))
        {
            string n = btn.gameObject.name;
            if (n.Contains("Start")) btn.onClick.AddListener(ShowFactionSelect);
        }

        // faction buttons
        humanButton.onClick.AddListener(() => SelectFaction(FactionType.Human));
        demiHumanButton.onClick.AddListener(() => SelectFaction(FactionType.DemiHuman));
        constructButton.onClick.AddListener(() => SelectFaction(FactionType.IntelligentConstruct));
        backButton.onClick.AddListener(HideFactionSelect);

        ShowMainMenu();
    }

    public void ShowFactionSelect()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (factionSelectOverlay) factionSelectOverlay.SetActive(true);
    }

    private void HideFactionSelect()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (factionSelectOverlay) factionSelectOverlay.SetActive(false);
    }

    private void SelectFaction(FactionType faction)
    {
        SelectedFaction = faction;
        SceneManager.LoadScene("GameScene");
    }
}
