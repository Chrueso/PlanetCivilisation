using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildMenuView : ScreenBase
{
    private StructureRecipeDatabaseSO structureRecepiesDatabase;
    public Button CloseButton;
    [SerializeField] private Transform layoutGroup;
    [SerializeField] private BuildElement buildElement;

    //MAKE IT A VISUAL LIST LIKE PLANET LIST WHERE U SHOW ALL THE STRUCTURES U CAN BUILD THEN WHEN U CLICK IT CHECKS REQUIRED RESOURCES AND BUILDS IT IF U HAVE THEM
    //SO LIKE STRUCTRE LIST ELEMENNT SPAWN BASED ON ALL STRUCTURES THEN ADD LISTENER

    //add listener which raises event to controller which checks
    //if player has resources and if so builds structure and closes menu, if not maybe show some kind of "not enough resources" message

    public void Init(StructureRecipeDatabaseSO structureRecepiesDatabase, Action<StructureType> onBuildElementClickedd)
    {
        this.structureRecepiesDatabase = structureRecepiesDatabase;

        foreach (StructureRecipeSO recipe in structureRecepiesDatabase.Recipes)
        {

        }
    }
}
