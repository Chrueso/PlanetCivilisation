using UnityEngine;
using UnityEngine.UI;

public class PlanetManagementInterface : MonoBehaviour
{
    private static PlanetManagementInterface instance;
    private Canvas canvas;
    public static PlanetManagementInterface main => instance;
    [SerializeField] private Button resourceButton;
    [SerializeField] private Button shipButton;
    [SerializeField] private Button populationButton;

    [SerializeField] private Image resourcePanel;
    [SerializeField] private Image shipPanel;
    [SerializeField] private Image populationPanel;

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



    private void ShowCanvas()
    {
        canvas.enabled = true;
    }

    private void HideCanvas()
    {
        canvas.enabled = false;
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

    public void ShowInterface() => ShowCanvas();
    public void HideInterface() => HideCanvas();
}
