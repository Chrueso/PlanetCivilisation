using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Move")]
public class AIMoveAction : AIAction
{
    [SerializeField] private AnimationCurve attackCurve;

    public override void Init(AIContext context)
    {
        attackCurve = new AnimationCurve(
            new Keyframe(0, 0), 
            new Keyframe(1, 1)); 
    }

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

            if (hex.Occupant != null && hex.Occupant is PlanetData planet)
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
                else if (isOwnedByMe)
                {
                    planetScore = 0.1f;
                }

                float distance = HexGridXZ< GridHex>.Distance(context.CurrentHex.GridPositionCube, hex.GridPositionCube);
                float distanceScore = 1 - Mathf.Clamp01(distance / context.Model.MoveRadius);
                hexScore = (planetScore * 0.8f) + (distanceScore * 0.1f);
            }
            else
            {
                float distanceFromLastVisitedHex = HexGridXZ<GridHex>.Distance(context.LastVisitedHex.GridPositionCube, hex.GridPositionCube);
                hexScore = Mathf.Clamp01(distanceFromLastVisitedHex / context.Model.MoveRadius) * 0.5f;
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

