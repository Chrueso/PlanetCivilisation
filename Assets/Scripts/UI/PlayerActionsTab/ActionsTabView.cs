using TMPro;
using UnityEngine.UI;

public class ActionsTabView : ScreenBase
{
    public override bool ShouldShowScreenRaycastBlocker => false;
    public override bool ShouldHonorBackButton => false;

    public Button InfoButton;
    public Button MoveButton;
    public Button ColonizeButton;
    public Button AttackButton;
    public Button DiplomacyButton;
    public Button StructureButton;
    public Button BuildButton;
    public Button StationShipsButton;

    public Button CloseButton;

    public TMP_Text hexOccupantText;
    public TMP_Text additionalInfoText;

    private GridHex selectedHex;
    private IEntityController entityController;
    private EntityModel entityModel;

    public void Init(IEntityController entityController)
    {
        this.entityController = entityController;
        this.entityModel = entityController.GetModel();
    }

    public void UpdateCurrentHex(GridHex selectedHex)
    {
        this.selectedHex = selectedHex;
    }

    public void HandleButtons()
    {
        if (selectedHex == null || entityController == null) return;

        HideAllButtons();

        bool isCurrentHex = selectedHex == entityModel.CurrentHex;
        bool inMoveRange = entityController.CheckIfHexIsInMoveRadius(selectedHex);

        // Move — not current hex and in range
        if (!isCurrentHex && inMoveRange)
            MoveButton.gameObject.SetActive(true);

        // No planet — nothing else to show
        if (selectedHex.Occupant is not PlanetData planet) return;

        // Has planet — always show info
        InfoButton.gameObject.SetActive(true);

        // Not on current hex — only info
        if (!isCurrentHex) return;

        bool isOwnedByMe = planet.FactionType == entityModel.FactionType;
        bool isOwnedByEnemy = planet.FactionType != FactionType.Nothing && !isOwnedByMe;
        bool isUninhabited = planet.FactionType == FactionType.Nothing;

        if (isOwnedByMe)
        {
            StationShipsButton.gameObject.SetActive(true);

            bool hasStructure = planet.Structures.Count > 0;
            BuildButton.gameObject.SetActive(!hasStructure);
            StructureButton.gameObject.SetActive(hasStructure);
        }
        else if (isOwnedByEnemy)
        {
            AttackButton.gameObject.SetActive(true);
            DiplomacyButton.gameObject.SetActive(true);
        }
        else if (isUninhabited)
        {
            ColonizeButton.gameObject.SetActive(true);
        }
    }

    public void HandleText()
    {
        if (selectedHex == null) return;

        hexOccupantText.text = $"Empty hex";
        additionalInfoText.text = "";

        if (selectedHex.Occupant != null)
        {
            if (selectedHex.Occupant is PlanetData planet)
            {
                hexOccupantText.text = planet.PlanetName;

                if (planet.FactionType == FactionType.Nothing)
                {
                    additionalInfoText.text = "Uninhabited";
                }
                else
                {
                    additionalInfoText.text = $"Colonized by {planet.FactionType}";

                    if (planet.FactionType == entityModel.FactionType)
                    {
                        additionalInfoText.text = $"Colonized by {planet.FactionType} (You)";
                    }
                }
            }
        }
    }

    protected override void OnShow()
    {
        HandleText();
        HandleButtons();
    }

    private void HideAllButtons()
    {
        MoveButton.gameObject.SetActive(false);
        InfoButton.gameObject.SetActive(false);
        AttackButton.gameObject.SetActive(false);
        ColonizeButton.gameObject.SetActive(false);
        DiplomacyButton.gameObject.SetActive(false);
        StructureButton.gameObject.SetActive(false);
        BuildButton.gameObject.SetActive(false);
        StationShipsButton.gameObject.SetActive(false);
    }
}
