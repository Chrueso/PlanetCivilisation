using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI structureName;
    [SerializeField] private TextMeshProUGUI recipeText;
    public Button ElementButton;
    public StructureType StructureType { private set; get; }

    public void Init(StructureRecipeSO recipe, Action<StructureType> onBuildElementClicked)
    {
        StructureType = recipe.StructureType;

        structureName.text = recipe.StructureType.ToString();
        recipeText.text = "Recipe: \n";
        foreach (var kvp in recipe.RecipeDict)
        {
            recipeText.text += $"{kvp.Key}: {kvp.Value} \n";
        }

        ElementButton.onClick.AddListener(() => onBuildElementClicked(recipe.StructureType));
    }
}
