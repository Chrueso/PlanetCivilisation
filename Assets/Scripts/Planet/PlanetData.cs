using System.Collections.Generic;
using UnityEngine;

public class PlanetData 
{
    public string Name;
    public List<ResourceType> ResourceTypes = new List<ResourceType>();
    public FactionType FactionType;
    public PlanetShapeSettings ShapeSettings;
    public PlanetColorSettings ColorSettings;

    public PlanetData(string planetName, List<ResourceType> resourceTypes, FactionType factionType, 
        PlanetShapeSettings shapeSettings, PlanetColorSettings colorSettings)
    {
        Name = planetName;
        ResourceTypes = resourceTypes;
        FactionType = factionType;
        ShapeSettings = shapeSettings;
        ColorSettings = colorSettings;
    }
}
