using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShipDatabaseSO", menuName = "Scriptable Objects/ShipDatabaseSO")]
public class ShipDatabaseSO : ScriptableObject
{
    [SerializeField] private List<ShipDataSO> ships = new();

    public Dictionary<ShipType, ShipDataSO> Ships { get; private set; }

    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        Ships = new Dictionary<ShipType, ShipDataSO>();

        foreach (var ship in ships)
        {
            if (ship == null) continue;

            Ships[ship.Type] = ship;
        }
    }

    public ShipDataSO GetShip(ShipType shipType)
    {
        if (Ships.TryGetValue(shipType, out ShipDataSO ship))
            return ship;

        Debug.LogError($"Ship type {shipType} not found in database");
        return null;
    }
}