using UnityEngine;
using System.Collections.Generic;
public class PlanetMasterScript : MonoBehaviour
{
    private Dictionary<string, Planet> planetDict;

    private void Awake()
    {
        planetDict = new Dictionary<string, Planet>();
    }

    private void GeneratePlanet()
    {

    }

}
