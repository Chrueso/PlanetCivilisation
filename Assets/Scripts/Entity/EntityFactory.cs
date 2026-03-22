using System.Collections.Generic;
using UnityEngine;

public class EntityFactory 
{
    private EntityView entityView;

    private List<FactionType> avaliableFactions;

    public EntityFactory(EntityView entityView)
    {
        this.entityView = entityView;

        avaliableFactions = new List<FactionType>() { FactionType.Human, FactionType.DemiHuman, FactionType.IntelligentConstruct};
    }

    public void CreateEntity(PlanetData homePlanet, FactionType factionType) //add entity preset as param then it makes the model
    {
        // if home plannet and faction type return
    }

    public EntityController CreatePlayer(PlanetData homePlanet, FactionType factionType, Vector3 position, out EntityData model, out EntityView view) //replace with preset 
    {
        model = new EntityData(homePlanet, factionType);
        view = Object.Instantiate(entityView);
        Vector3 spawnPos = position;
        spawnPos.y = model.yValue;
        view.transform.position = spawnPos;
        EntityController controller = new EntityController(model, view);
        model.CurrentHex = homePlanet.CurrentHex;

        avaliableFactions.Remove(factionType);

        return controller;
    }
}
