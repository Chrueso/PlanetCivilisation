using System.Collections.Generic;
using UnityEngine;

public class BattleVisualController : MonoBehaviour
{
    [SerializeField] ShipModelDatabaseSO shipModelDb;
    [SerializeField] GameObject shipBattlePrefab;
    List<GameObject> attackingShipModels;
    List<GameObject> defendingShipModels;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnShips(Dictionary<ShipType, int> attackingShips, Dictionary<ShipType , int> defendingShips, FactionType attackerFaction , PlanetData targetPlanet)
    {
        attackingShipModels = new();
        defendingShipModels = new();

        foreach(var attackingShip in attackingShips.Keys)
        {
            GameObject ship = Instantiate(shipBattlePrefab, this.transform);
            //make the ship & set its model
            attackingShipModels.Add(ship);
            //change this to attacker faction
            ship.GetComponent<ShipBattleView>().ChangeModel(shipModelDb.GetModel(targetPlanet.FactionType,attackingShip));
            ship.transform.position = targetPlanet.CurrentHex.WorldPosition;
        }

        foreach (var defendingShip in defendingShips.Keys)
        {
            GameObject ship = Instantiate(shipBattlePrefab, this.transform);
            //make the ship & set its model
            defendingShipModels.Add(ship);
            ship.GetComponent<ShipBattleView>().ChangeModel(shipModelDb.GetModel(targetPlanet.FactionType, defendingShip));
            ship.transform.position = targetPlanet.CurrentHex.WorldPosition;
        }
    }
}
