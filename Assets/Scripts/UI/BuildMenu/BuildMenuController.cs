using UnityEngine;

public class BuildMenuController 
{
    private BuildMenuView view;
    private StructureRecipeDatabaseSO structureRecepiesDatabase;
    private IEntityController entityController;
    private EntityModel entityModel;
    private PlanetData currentPlanet;
    public BuildMenuController(BuildMenuView view, StructureRecipeDatabaseSO structureRecepiesDatabase)
    {
        this.view = view;
        this.structureRecepiesDatabase = structureRecepiesDatabase;  

        ConnectView();
    }

    public void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
        view.Init(structureRecepiesDatabase, HandleBuildElementClicked);
    }

    public void OpenView(PlanetData planet, IEntityController entityController)
    {
        this.entityController = entityController;
        entityModel = entityController.GetModel();
        this.currentPlanet = planet;
        view.UpdateView(entityModel, planet);
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }

    public void HandleBuildElementClicked(StructureType structureType)
    {
        view.UpdateView(entityModel, currentPlanet);

        if (entityController.TryBuildStructure(currentPlanet, structureType))
        {
            CloseView();
        }
        else
        {
            Debug.Log("Not enough resources to build " + structureType);
        }
    }
}
