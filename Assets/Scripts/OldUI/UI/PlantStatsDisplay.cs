using TMPro;
using UnityEngine;

public class PlanetStatsDisplay : MonoBehaviour
{
    [Header("Resource Text References")]
    [SerializeField] private TextMeshProUGUI resourceNameText;
    [SerializeField] private TextMeshProUGUI resourceCountText;
    [SerializeField] private TextMeshProUGUI structureCountText;
    [SerializeField] private TextMeshProUGUI resourcePerTurnText;

    [Header("Display Settings")]
    [SerializeField] private string resourcePrefix = "Resources: ";
    [SerializeField] private string structurePrefix = "Structures: ";
    [SerializeField] private string resourcePerTurnPrefix = "+";
    [SerializeField] private string resourcePerTurnSuffix = "Per Turn";

    [Header("Stats Update Settings")]
    // idk will be used later maybe
    [SerializeField] private float updateInterval = 0.1f;
    private PlanetData currentPlanet;
    private float updateTimer;

    private void Update()
    {
        if (currentPlanet != null)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer >= updateInterval)
            {
                UpdateDisplay();
                updateTimer = 0f;
            }
        }
    }

    public void SetPlanet(PlanetData planet)
    {
        currentPlanet = planet;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        /* the way planet data is structured changed so will have to redo mb
        if (currentPlanet == null)
        {
            ClearDisplay();
            return;
        }
        if (resourceNameText != null)
            resourceNameText.text = $"{currentPlanet.ResourceName}";

        if (resourceCountText != null)
            resourceCountText.text = $"{resourcePrefix}{currentPlanet.Resource}";

        if (structureCountText != null)
            structureCountText.text = $"{structurePrefix}{currentPlanet.Structures}";

        if (resourcePerTurnText != null)
            resourcePerTurnText.text = $"{resourcePerTurnPrefix}{currentPlanet.ResourcePerTurn} {resourcePerTurnSuffix}";
        */
    }

    private void ClearDisplay()
    {
        /*
        if (resourceNameText != null)
            resourceNameText.text = $" ";

        if (resourceCountText != null)
            resourceCountText.text = resourcePrefix + "0";

        if (structureCountText != null)
            structureCountText.text = structurePrefix + "0";

        if (resourcePerTurnText != null)
            resourcePerTurnText.text = $"{resourcePerTurnPrefix}0 {resourcePerTurnSuffix}";
        */
    }

    public void ForceUpdate()
    {
        //UpdateDisplay();
    }
}
