using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfigSO", menuName = "Scriptable Objects/GameConfigSO")]
public class GameConfigSO : ScriptableObject
{
    [Header("General Config")]
    public int MaxTurns = 10;
    public int MaxAP = 4;
    public int MaxPlanetsNeeded = 5;

    [Header("Planet Specific Config")]
    public int MinWorkerShipNeededForColonize = 1;
    public int MaxStationedWorkerShips = 10;
    public int MaxStationedAssaultShips = 10;
    public int DefenseStructureDefenseValue = 2;

    [Header("Entity Specific Config")]
    public int MaxHeldWorkerShips = 10;
    public int MaxHeldScoutShips = 10;
    public int MaxHeldAssaultShips = 10;
    public int StartingResourcesAmount = 10;
    public int StartingShipsAmount = 10;

    [Header("Ship Specific Config")]
    public int AssaultShipAttackValue = 1;
    public int WorkerShipEfficacyValue = 1;
    public int ScoutShipRangeValue = 10;
    public int MoveRadius = 5;


    private void OnValidate()
    {
        MaxTurns = Mathf.Max(MaxTurns, 1);
        MaxAP = Mathf.Max(MaxAP, 1);
        MaxPlanetsNeeded = Mathf.Max(MaxPlanetsNeeded, 1);

        MinWorkerShipNeededForColonize = Mathf.Max(MinWorkerShipNeededForColonize, 1);
        MaxStationedWorkerShips = Mathf.Max(MaxStationedWorkerShips, 1);
        MaxStationedAssaultShips = Mathf.Max(MaxStationedAssaultShips, 1);
        DefenseStructureDefenseValue = Mathf.Max(DefenseStructureDefenseValue, 1);

        MaxHeldWorkerShips = Mathf.Max(MaxHeldWorkerShips, 1);
        MaxHeldScoutShips = Mathf.Max(MaxHeldScoutShips, 1);
        MaxHeldAssaultShips = Mathf.Max(MaxHeldAssaultShips, 1);
        StartingResourcesAmount = Mathf.Max(StartingResourcesAmount, 1);
        StartingShipsAmount = Mathf.Max(StartingShipsAmount, 1);

        AssaultShipAttackValue = Mathf.Max(AssaultShipAttackValue, 1);
        WorkerShipEfficacyValue = Mathf.Max(WorkerShipEfficacyValue, 1);
        ScoutShipRangeValue = Mathf.Max(ScoutShipRangeValue, 1);
        MoveRadius = Mathf.Max(MoveRadius, 1);
    }
}
