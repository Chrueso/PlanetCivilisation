using System.Collections.Generic;
using UnityEngine;

public class EntityFactory 
{
    private MapGrid mapGrid;
    private EntityView entityView;
    private CommandInvoker commandInvoker;

    private List<FactionType> avaliableFactions;

    public EntityFactory(MapGrid mapGrid, EntityView entityView, CommandInvoker commandInvoker)
    {
        this.mapGrid = mapGrid;
        this.entityView = entityView;
        this.commandInvoker = commandInvoker;

        avaliableFactions = new List<FactionType>() { FactionType.Human, FactionType.DemiHuman, FactionType.IntelligentConstruct};
    }

    public void CreateEntity(PlanetData homePlanet, FactionType factionType) //add entity preset as param then it makes the model
    {
        // if home plannet and faction type return
    }

    public PlayerController CreatePlayer(PlanetData homePlanet, FactionType factionType, Vector3 position) //replace with preset 
    {
        EntityModel model = new EntityModel(homePlanet, factionType);
        EntityView view = Object.Instantiate(entityView);
        Vector3 spawnPos = position;
        spawnPos.y = model.yValue;
        view.transform.position = spawnPos;
        PlayerController controller = new PlayerController(model, view, mapGrid, commandInvoker);
        model.CurrentHex = homePlanet.CurrentHex;

        avaliableFactions.Remove(factionType);
        return controller;
    }
}
