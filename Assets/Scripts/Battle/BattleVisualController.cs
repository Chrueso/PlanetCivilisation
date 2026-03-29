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

    void fanOutShips(List<GameObject> ships,PlanetData targetPlanet, float radius, float startAngle, float endAngle)
    {
        int count = ships.Count;

        for(int i = 0; i < count; i++)
        {
            float t = count == 1 ? 0.5f : (float)i / (count - 1);
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            Vector3 shipPosition = GetOrbitPosition(targetPlanet, radius, angle);

            ships[i].TryGetComponent<ShipBattleView>(out ShipBattleView shipVisual);
            if (shipVisual != null) shipVisual.Move(shipPosition);

            // rotate to face planet
            Vector3 dir = (targetPlanet.CurrentHex.WorldPosition - shipPosition).normalized;
            Quaternion rot = Quaternion.LookRotation(dir);
        }

        
    }

    Vector3 GetOrbitPosition(PlanetData targetPlanet, float radius, float angle)
    {
        Vector3 center = targetPlanet.CurrentHex.WorldPosition;
        float rad = angle * Mathf.Deg2Rad;
        float finalRadius = targetPlanet.View.ShapeSettings.PlanetRadius + radius;

        float x = Mathf.Cos(rad) * finalRadius;
        float z = Mathf.Sin(rad) * finalRadius;

        return new Vector3(center.x + x, center.y, center.z + z);
    }
}
