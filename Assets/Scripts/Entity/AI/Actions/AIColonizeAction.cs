using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Colonize")]
public class AIColonizeAction : AIAction
{
    public override void Init(AIContext context)
    {
    }

    public override float CalculateUtility(AIContext context)
    {
        EntityModel model = context.Model;
        EntityController controller = context.Controller;
        GridHex currentHex = model.CurrentHex;
        PlanetData planet = currentHex.Occupant as PlanetData;
        bool isOnPlanet = planet != null;
        bool isUninhabited = isOnPlanet && planet.FactionType == FactionType.Nothing;

        if (isUninhabited) return 1;

        return -1;
    }

    public override void Execute(AIContext context)
    {
        PlanetData planet = context.Model.CurrentHex.Occupant as PlanetData;
        context.Controller.TryColonize(planet);
    }
}
