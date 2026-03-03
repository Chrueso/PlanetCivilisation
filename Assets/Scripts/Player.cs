using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGridHexOccupant
{
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public Dictionary<ShipTypeSO, int> Ships { get; private set; } = new Dictionary<ShipTypeSO, int>();
    public FactionType FactionType { get; private set; }
    public List<PlanetData> OwnedPlanets { get; private set; } = new List<PlanetData>();
    public PlanetData HomePlanet { get; private set; }

    public GridHex CurrentHex { get; set; }

    float playerY = 3f;

    public void Init(PlanetData homePlanet, FactionType factionType)
    {
        HomePlanet = homePlanet;
        OwnedPlanets.Add(homePlanet);
        FactionType = factionType;
        CurrentHex = homePlanet.CurrentHex;
        transform.position = new Vector3(CurrentHex.WorldPosition.x, playerY, CurrentHex.WorldPosition.z);
        Ships[HardcodeReference.Instance.ScoutShip] = 1;
        Ships[HardcodeReference.Instance.AttackShip] = 1;
        Ships[HardcodeReference.Instance.WorkerShip] = 1;
    }

    public void MoveToHex(GridHex hex)
    {
        CurrentHex = hex;
        transform.position = new Vector3(CurrentHex.WorldPosition.x, playerY, CurrentHex.WorldPosition.z); ;
        //Chnage to some tween / coroutine / move thing action yes
    }

}
