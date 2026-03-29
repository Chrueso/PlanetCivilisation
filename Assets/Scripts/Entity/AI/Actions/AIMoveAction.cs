using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[CreateAssetMenu(menuName = "AI/Actions/Move")]
public class AIMoveAction : AIAction
{
    [SerializeField] private AnimationCurve curve;

    public override void Init(AIContext context)
    {
        curve = new AnimationCurve(
            new Keyframe(0, 1),  // at normalized distance 0 utility is 1
            new Keyframe(1, 0)); // at normalized distance 1 utility is 0
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

                if (isUninhabited)
                {
                    //Check resource
                    hexScore = 1; 
                }
                else if (isOwnedByEnemy)
                {
                    hexScore = curve.Evaluate(context.AttackPower/ planet.CalculateDefensePower()); // prefer if I have higher attack power
                }
                else if (isOwnedByMe)
                {
                    hexScore = 0.5f;
                }

                //hexScore + distanceScoring + visited; distaancee from last visited hex 
            }
            else
            {
                hexScore = 0.5f; // neutral score for empty hexes
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

