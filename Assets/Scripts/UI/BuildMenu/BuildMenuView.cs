using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class BuildMenuView : ScreenBase
{
    public Button CloseButton;
    [SerializeField] private Transform layoutGroup;
    [SerializeField] private BuildElement buildElement;
    private List<BuildElement> buildElements = new List<BuildElement>();

    public void Init(StructureRecipeDatabaseSO structureRecepiesDatabase, Action<StructureType> onBuildElementClicked)
    {
        foreach (StructureRecipeSO recipe in structureRecepiesDatabase.Recipes)
        {
            BuildElement element = Instantiate(buildElement, layoutGroup);
            element.Init(recipe, onBuildElementClicked);
            buildElements.Add(element);
        }
    }

    public void UpdateView(EntityModel entityModel, PlanetData planet)
    {
        //If not enough resource to build change visual of elemnent maybe grey out or something
        //Alos if planet has structure grey out certain elementsd

        foreach (BuildElement element in buildElements)
        {
            bool isDefense = element.StructureType == StructureType.Defense;
            bool hasAnyStructure = planet.Structures.Count > 0;
            bool hasDefense = planet.Structures.Contains(StructureType.Defense);

            bool shouldDisable = hasAnyStructure && !(isDefense && !hasDefense);
            element.ElementButton.interactable = !shouldDisable;
        }
    }
}
