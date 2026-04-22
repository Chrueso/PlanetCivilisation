using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetListElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private TextMeshProUGUI resourceGain;
    [SerializeField] private TextMeshProUGUI structures;
    [SerializeField] private Button planetButton;
    [SerializeField] private RawImage planetIcon;


    public void Init(PlanetData planetData, Action<PlanetData> onPlanetButtonClicked)
    {
        planetIcon.texture = planetData.View.Icon;
        planetName.text = planetData.PlanetName;

        // Display each resource in planet with new line
        string resourceString = "Resource Gain: \n";
        foreach (var resource in planetData.GeneratedResource)
        {
            resourceString += $"{resource.Key}: {resource.Value} \n";
        }
        resourceGain.text = resourceString;
        
        structures.text = $"Structures: {planetData.Structures.Count}";

        planetButton.onClick.RemoveAllListeners();
        planetButton.onClick.AddListener(() =>
        {
            onPlanetButtonClicked?.Invoke(planetData);
        });
    }
}
