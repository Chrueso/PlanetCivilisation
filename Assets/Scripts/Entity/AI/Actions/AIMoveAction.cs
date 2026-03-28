using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Move")]
public class AIMoveAction : AIAction
{
    Dictionary<int, GridHex> hexScores;

    EntityModel model;
    EntityController controller;

    AnimationCurve curve;

    public override void Init(AIContext context)
    {
        model = context.Model;
        controller = context.Controller;

        curve = new AnimationCurve(
            new Keyframe(0, 1),  // at normalized distance 0 utility is 1
            new Keyframe(1, 0)); // at normalized distance 1 utility is 0
    }

    public override float CalculateUtility(AIContext context)
    {
        GridHex currentHex = model.CurrentHex;
        var planet = currentHex.Occupant as PlanetData;
        bool isOnPlanet = planet != null;
        bool isUninhabited = isOnPlanet && planet.FactionType == FactionType.Nothing;
        bool isOwnedByMe = isOnPlanet && planet.FactionType == controller.GetFaction();
        bool isOwnedByEnemy = isOnPlanet && !isUninhabited && !isOwnedByMe;

        if (!isOnPlanet) return 1;
        else if (isUninhabited) return 0; 
        else if (isOwnedByMe) return 1;
        else if (isOwnedByEnemy)
        {
            int defensePower = planet.CalculateDefensePower();
            int attackPower = model.CalculateAttackPower();

            return curve.Evaluate((float)attackPower / defensePower); //if attack 1 defense 0 return 1 so move
        }

        return 0;
    }

    public override void Execute(AIContext context)
    {
        GridHex currentHex = model.CurrentHex;
        GridHex bestHex = null;
        float highestHexScore = float.MinValue;

        foreach (GridHex hex in controller.HexesInMoveRadius)
        {
            if (hex == currentHex) continue;

            float hexScore = 0;

            if (hex.Occupant is PlanetData planet)
            {
                bool isUninhabited = planet.FactionType == FactionType.Nothing;
                bool isOwnedByMe = planet.FactionType == controller.GetFaction();
                bool isOwnedByEnemy = !isUninhabited && !isOwnedByMe;
                if (isUninhabited)
                {
                    hexScore = 1; // prefer uninhabited or owned by me
                }
                else if (isOwnedByEnemy)
                {
                    int defensePower = planet.CalculateDefensePower();
                    int attackPower = model.CalculateAttackPower();
                    hexScore = curve.Evaluate((float)attackPower / defensePower); // prefer if I have higher attack power
                }
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

        controller.TryMove(bestHex);
    }
}
