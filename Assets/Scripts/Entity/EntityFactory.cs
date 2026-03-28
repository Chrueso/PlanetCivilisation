using System.Collections.Generic;
using UnityEngine;

public class EntityFactory 
{
    private EntityView entityView;

    private List<FactionType> avaliableFactions;

    ShipDatabaseSO shipDatabase;

    public EntityFactory(ShipDatabaseSO shipDatabase, EntityView entityView) //We want different ship view for player and the ai so maybe a database of SOs later?
    {
        this.shipDatabase = shipDatabase;
        this.entityView = entityView;

        avaliableFactions = new List<FactionType>() { FactionType.Human, FactionType.DemiHuman, FactionType.IntelligentConstruct};
    }

    public EntityController CreateAI(out AIBrain brain, PlanetData homePlanet, List<AIAction> actions, FactionType factionType = FactionType.Nothing)
    {
        brain = null;

        if (homePlanet == null || homePlanet.FactionType != FactionType.Nothing)
        {
            Debug.Log(this + "Failed to create ai");
            return null;
        }

        if (factionType == FactionType.Nothing)
        {
            if (avaliableFactions.Count == 0)
            {
                Debug.Log(this + "No available factions left!");
                return null;
            }
            int index = Random.Range(0, avaliableFactions.Count);
            factionType = avaliableFactions[index];
        }

        avaliableFactions.Remove(factionType);

        EntityModel model = new EntityModel(shipDatabase, homePlanet, factionType);
        model.CurrentHex = homePlanet.CurrentHex;

        EntityView view = Object.Instantiate(entityView);

        Vector3 spawnPos = homePlanet.CurrentHex.WorldPosition;
        spawnPos.y = model.yValue;
        view.transform.position = spawnPos;

        EntityController controller = new EntityController(model, view);

        brain = new AIBrain(controller, actions);

        return controller;
    }

    public EntityController CreatePlayer(PlanetData homePlanet, FactionType factionType) //replace with preset 
    {
        if (homePlanet == null || homePlanet.FactionType != FactionType.Nothing)
        {
            Debug.Log(this + "Failed to create player");
            return null;
        }

        EntityModel model = new EntityModel(shipDatabase,homePlanet, factionType);
        model.CurrentHex = homePlanet.CurrentHex;

        EntityView view = Object.Instantiate(entityView);

        Vector3 spawnPos = homePlanet.CurrentHex.WorldPosition;
        spawnPos.y = model.yValue;
        view.transform.position = spawnPos;

        EntityController controller = new EntityController(model, view);

        avaliableFactions.Remove(factionType);

        return controller;
    }
}
