using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Planet Data", menuName = "Planet/Custom Planet Data")]
public class CustomPlanetSO : ScriptableObject
{
    public string PlanetName;

    //public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();

    public FactionType FactionType;

    //public List<Structure> Structures { get; private set; }

    public PlanetShapeSettings ShapeSettings;
    public PlanetColorSettings ColorSettings;
}
