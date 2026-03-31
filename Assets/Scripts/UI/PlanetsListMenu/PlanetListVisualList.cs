using UnityEngine;
using TMPro;

public class PlanetListVisualList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private TextMeshProUGUI resourceGain;
    [SerializeField] private TextMeshProUGUI structures;

    public void setup(PlanetData planetData)
    {
        planetName.text = planetData.PlanetName;

        // Display each resource in planet with new line
        string resourceString = "Resource Gain: \n";
        foreach (var resource in planetData.GeneratedResource)
        {
            resourceString += $"{resource.Key}: {resource.Value} \n";
        }
        resourceGain.text = resourceString;

        structures.text = $"Structures: {planetData.Structures.Count}";
    }
}
