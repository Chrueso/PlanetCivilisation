using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Move")]
public class AIMoveAction : AIAction
{
    [SerializeField] private AnimationCurve attackCurve;
    [SerializeField] private AnimationCurve centerCurve;

    public override float CalculateUtility(AIContext context)
    {
        return 0.5f;
    }

    public override void Execute(AIContext context)
    {
        GridHex bestHex = null;
        float highestHexScore = float.MinValue;

        //Calculate score for each hex in move radius and pick the one with highest score
        foreach (GridHex hex in context.HexesInMoveRadius)
        {
            if (hex == context.CurrentHex) continue;

            float hexScore = 0;

            if (hex.Occupant != null && hex.Occupant is PlanetData planet && planet.FactionType != context.Model.FactionType)
            {
                bool isUninhabited = planet.FactionType == FactionType.Nothing;
                bool isOwnedByMe = planet.FactionType == context.Model.FactionType;
                bool isOwnedByEnemy = !isUninhabited && !isOwnedByMe;

                float planetScore = 0;
                if (isUninhabited)
                {
                    //Check resource
                    planetScore = 1; 
                }
                else if (isOwnedByEnemy)
                {
                    int planetDefense = planet.CalculateDefensePower();
                    planetScore = planetDefense > 0 ? attackCurve.Evaluate((float)context.AttackPower / planetDefense) : 1f; 
                }

                float distance = HexGridXZ< GridHex>.Distance(context.CurrentHex.GridPositionCube, hex.GridPositionCube);
                float closestDistanceScore = 1 - Mathf.Clamp01(distance / context.Model.MoveRadius);

                hexScore = (planetScore * 0.8f) + (closestDistanceScore * 0.1f);
            }
            else
            {
                float distanceFromLastVisitedHex = HexGridXZ<GridHex>.Distance(context.LastVisitedHex.GridPositionCube, hex.GridPositionCube);
                //float distanceFromLastVisitedHexScore = Mathf.Clamp01(distanceFromLastVisitedHex / context.Model.MoveRadius);

                if (distanceFromLastVisitedHex < context.Model.MoveRadius) continue; //waste of ap to move less

                float distanceFromCenter = HexGridXZ<GridHex>.Distance(hex.GridPositionCube, context.MapGrid.Grid.GetAproxCenterGridObject.GridPositionCube);
                float maxDistanceFromCenter = context.MapGrid.Grid.Width / 2f;
                float centerScore = 1f - centerCurve.Evaluate(distanceFromCenter / maxDistanceFromCenter);

                //hexScore += distanceFromLastVisitedHexScore * centerScore * 0.1f;

                hexScore += centerScore * 0.1f;
            }

            if (hexScore > highestHexScore)
            {
                highestHexScore = hexScore;
                bestHex = hex;
            }
        }

        context.VisitedHexes.Add(context.CurrentHex);
        context.LastVisitedHex = context.CurrentHex;
        context.Controller.TryMove(bestHex);
    }

}

