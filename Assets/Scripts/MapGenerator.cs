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
        mapGrid = null;
        homePlanet = null;

        if (settings == null)
        {
            Debug.LogError("MapGenerator: MapSettings is null");
            return;
        }

        if (settings.MapGridPrefab == null)
        {
            Debug.LogError("MapGenerator: MapGridPrefab is null in MapSettings");
            return;
        }

        if (settings.MapDimensions.x <= 0 || settings.MapDimensions.y <= 0)
        {
            Debug.LogError("MapGenerator: MapDimensions must be > 0");
            return;
        }

        // Instantiate grid
        mapGrid = Object.Instantiate(settings.MapGridPrefab);
        mapGrid.GenerateGrid(settings.MapDimensions.x, settings.MapDimensions.y, settings.MapCellSize);

        // Create PlanetMapGenerator
        planetMapGenerator = new PlanetMapGenerator(
            mapGrid,
            settings.RadiusBetweenPlanets,
            settings.MaxPlanets,
            settings.HomePlanetData,
            planetGenerator,
            rng
        );

        // Generate planets and assign factions
        planetMapGenerator.GeneratePlanets(out homePlanet);
        planetMapGenerator.AssignFactionToPlanets(homePlanet);
    }
}
