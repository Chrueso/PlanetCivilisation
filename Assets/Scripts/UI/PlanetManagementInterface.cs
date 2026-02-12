using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.UI;

public class PlanetManagementInterface : MonoBehaviour
{
    private static PlanetManagementInterface instance;
    private Canvas canvas;
    public static PlanetManagementInterface main => instance;

    [Header("UI Buttons")]
    [SerializeField] private Button resourceButton;
    [SerializeField] private Button shipButton;
    [SerializeField] private Button populationButton;

    [Header("UI Panels")]
    [SerializeField] private Image resourcePanel;
    [SerializeField] private Image shipPanel;
    [SerializeField] private Image populationPanel;

    [Header("Planet Stats Display")]
    [SerializeField] private PlanetStatsDisplay planetStatsDisplay; // Check Manager object if reference missing

    private Planet currentSelectedPlanet;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        resourceButton.onClick.AddListener(ToggleResourcePanel);
        shipButton.onClick.AddListener(ToggleShipPanel);
        populationButton.onClick.AddListener(TogglePopuPanel);
        canvas = GetComponent<Canvas>();
    }

    private void ToggleResourcePanel()
    {
        SetAllPanelsInactive();
        resourcePanel.gameObject.SetActive(true);
    }

    private void ToggleShipPanel()
    {
        SetAllPanelsInactive();
        shipPanel.gameObject.SetActive(true);
    }

    private void TogglePopuPanel()
    {
        SetAllPanelsInactive();
        populationPanel.gameObject.SetActive(true);
    }

    private void SetAllPanelsInactive()
    {
        resourcePanel.gameObject.SetActive(false);
        shipPanel.gameObject.SetActive(false);
        populationPanel.gameObject.SetActive(false);
    }

    public void ShowInterface(Planet planet)
    {
        currentSelectedPlanet = planet;
        if (currentSelectedPlanet != null)
        {
            planetStatsDisplay.SetPlanet(currentSelectedPlanet);
        }

        canvas.enabled = true;
        ToggleResourcePanel(); // default to resource
    }

    public void HideInterface()
    {
        currentSelectedPlanet = null;
        canvas.enabled = false;
    }
}
