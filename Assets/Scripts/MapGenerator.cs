using UnityEngine;

public class MapGenerator 
{
    private MapSettings settings;
    private PlanetMapGenerator planetMapGenerator;

    private PlanetGenerator planetGenerator;

    public MapGenerator(MapSettings mapSettings, PlanetGenerator planetGenerator)
    {
        this.settings = mapSettings;
        this.planetGenerator = planetGenerator;
    }

    public void GenerateMap(out MapGrid mapGrid, out PlanetData homePlanet, System.Random rng)
    {
        mapGrid = Object.Instantiate(settings.MapGridPrefab);
        mapGrid.GenerateGrid(settings.MapDimensions.x, settings.MapDimensions.y, settings.MapCellSize);

        planetMapGenerator = new PlanetMapGenerator(
            settings.RadiusBetweenPlanets, 
            settings.MaxPlanets, 
            settings.HomePlanetData, 
            planetGenerator, 
            rng
            );

        planetMapGenerator.GeneratePlanets(out homePlanet);
        planetMapGenerator.AssignFactionToPlanets(homePlanet);
    }
    
}
