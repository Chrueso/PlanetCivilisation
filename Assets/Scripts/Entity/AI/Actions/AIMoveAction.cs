
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Move")]
public class AIMoveAction : AIAction
{
    [SerializeField] private AnimationCurve attackCurve;
    [SerializeField] private AnimationCurve centerCurve;
    [SerializeField] private AnimationCurve maxShipCurve; //if shipcount closer to max ship then higher score

    public override float CalculateUtility(AIContext context)
    {
        return 0.5f;
    }

    public override void Execute(AIContext context)
    {
        GridHex bestHex = null;
        float highestHexScore = float.MinValue;

        GridHex centerHex = context.MapGrid.Grid.GetAproxCenterGridObject;
        float maxDistanceFromCenter = context.MapGrid.Grid.Width * 0.5f;
        Vector3 lastDirection = context.LastHex != context.CurrentHex
            ? ((Vector3)(context.CurrentHex.GridPositionCube - context.LastHex.GridPositionCube)).normalized
            : Vector3.forward;

        //Calculate score for each hex in move radius and pick the one with highest score
        foreach (GridHex hex in context.HexesInMoveRadius)
        {
            if (hex == context.CurrentHex) continue;

            float hexScore = 0;

            if (hex.Occupant != null && hex.Occupant is PlanetData planet)
            {
                bool isUninhabited = planet.FactionType == FactionType.Nothing;
                bool isOwnedByMe = planet.FactionType == context.Model.FactionType;
                bool isOwnedByEnemy = !isUninhabited && !isOwnedByMe;

                float planetScore = 0;
                if (isUninhabited) //usualy u want to go here
                {    
                    planetScore = context.Model.EnoughShips(ShipType.Worker, context.GameConfig.MinWorkerShipNeededForColonize) ? 1f : 0f; 
                }
                else if (isOwnedByMe) //go back to build
                {
                    planetScore = context.HasResourceToBuildStructures && planet.Structures.Count == 0 ? 1f : 0f;

                    //if planet ship count is less than 20 percent of current ships and ship count is close to max

                    int shipCount = context.Model.Ships[ShipType.Attacker] + context.Model.Ships[ShipType.Worker];
                    int maxShipCount = context.GameConfig.MaxStationedAssaultShips + context.GameConfig.MaxStationedWorkerShips;
                    int planetShipCount = planet.GetShipCount(ShipType.Attacker) + planet.GetShipCount(ShipType.Worker);

                    if (planetShipCount <= shipCount * 0.2f)
                    {
                        // scor eiddk add
                    }

                }
                else if (isOwnedByEnemy) //based on ship
                {
                    int planetDefense = planet.CalculateDefensePower();
                    planetScore = planetDefense > 0 ? Mathf.Clamp01(attackCurve.Evaluate((float)context.AttackPower / planetDefense)) : 1f;
                }

                float distance = HexGridXZ< GridHex>.Distance(context.CurrentHex.GridPositionCube, hex.GridPositionCube);
                float closestDistanceScore = 1 - Mathf.Clamp01(distance / context.Model.MoveRadius);

                hexScore = (planetScore * 0.9f) + (closestDistanceScore * 0.1f * planetScore); //prio closest if both planetscores equally similar
            }
            else //For empty hexes and own planet prefer hexes that are farther from recently visited hexes and closer to center of the map
            {
                float distanceFromCurrentHex = HexGridXZ<GridHex>.Distance(hex.GridPositionCube, context.CurrentHex.GridPositionCube);
                if (distanceFromCurrentHex < context.Model.MoveRadius) continue; // waste of ap

                float distanceFromLastHex = HexGridXZ<GridHex>.Distance(hex.GridPositionCube, context.LastHex.GridPositionCube);
                float lastHexScore = Mathf.Clamp01(distanceFromLastHex / context.Model.MoveRadius);

                Vector3 candidateDirection = ((Vector3)(hex.GridPositionCube - context.CurrentHex.GridPositionCube)).normalized;
                float directionScore = (Vector3.Dot(lastDirection, candidateDirection) + 1f) * 0.5f; // remap -1,1 to 0,1

                float distanceFromCenter = HexGridXZ<GridHex>.Distance(hex.GridPositionCube, centerHex.GridPositionCube);
                float centerScore = Mathf.Clamp01(centerCurve.Evaluate(distanceFromCenter / maxDistanceFromCenter));

                float hexRecencyScore = context.VisitedHexes.ContainsKey(hex) ? context.VisitedHexes[hex] : 0f; // prefer unvisited hexes

                hexScore += Mathf.Clamp01((lastHexScore * directionScore * centerScore * 0.5f) - hexRecencyScore);
            }

            //Debug.Log("Hexscore " + hexScore);

            if (hexScore > highestHexScore)
            {
                highestHexScore = hexScore;
                bestHex = hex;
            }
        }

        context.LastHex = context.CurrentHex;
        context.AddLastVisitedHex(context.CurrentHex);
        context.Controller.TryMove(bestHex);
    }

}

