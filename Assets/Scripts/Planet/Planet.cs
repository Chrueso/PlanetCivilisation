using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Rock,
    Copper,
    Iron,
}

public class Planet
{
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();

    public int Structures { get; private set; }

    public void Init()
    {

    }

    private void IncreaseResource()
    {
        if (Resources.Count > 0)
        {
            foreach (var resource in Resources.Keys)
            {
                Resources[resource] += 1;
            }
        }
    }

    public void PlanetResourceGain() => IncreaseResource();
}
