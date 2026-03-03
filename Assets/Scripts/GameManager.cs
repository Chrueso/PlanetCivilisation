using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random GalaxyRNG { get; private set; }
    public System.Random PlanetRNG { get; private set; } // Maybe il change to data struct which combines all rng needed later -Zen

    [SerializeField] public MapGrid mapGridPrefab;
    [SerializeField] private Player playerPrefab;

    public MapGrid MapGrid { get; private set; }

    private void Start()
    {
        GenerateSeed();

        // I think maybe in future have 1 world/map gen which initialize both and pass references to all generators need
        // PlanetManager being singleton cause alot dependency? maybe
        // Interactions should be trygetcomponent i think better? not sure 

        MapGrid = Instantiate(mapGridPrefab, Vector3.zero, Quaternion.identity); //CHANGE LATER NEED SOME SORT OF MAP GEN TOO MANYH REFERENCES
        MapGrid.GenerateGrid(50, 50, 6);

        PlanetManager.Instance.GeneratePlanets(MapGrid, PlanetRNG, out PlanetData homePlanet);

        Player player = Instantiate(playerPrefab);
        player.Init(homePlanet, FactionType.Human);

        Vector3 homeplanetPos = homePlanet.CurrentHex.WorldPosition;
        CameraController.Instance.CameraInstance.transform.position =  new Vector3(homeplanetPos.x, CameraController.Instance.CameraInstance.transform.position.y, homeplanetPos.z);
    }

    private void GenerateSeed()
    {
        // First generate galaxy name which is the seed for everything
        GalaxyName = GalaxyGenerator.GenerateGalaxyName();
        SeedInt = SeedUtil.StringToHashCode(GalaxyName);

        // Then using the galaxy seed to generate seeds for the things in game ask me if need clarity -Zen
        // MoonRNG etc...
        // Basically anything that needs a seed for generation in game should use this pattern
        GalaxyRNG = new System.Random(SeedInt);
        PlanetRNG = new System.Random(SeedInt + 1000);

        Debug.Log($"Generated Galaxy Name: {GalaxyName} with Seed: {SeedInt}");
    }

}
