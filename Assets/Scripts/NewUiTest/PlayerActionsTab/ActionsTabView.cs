using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionsTabView : ScreenBase
{
    public Button InfoButton;
    public Button MoveButton;
    public Button ColonizeButton;
    public Button AttackButton;
    public Button DiplomacyButton;
    public Button BuildStructureButton;

    public Button CloseButton;

    public TMP_Text hexText;
    public TMP_Text hexOccupantText;

    private GridHex selectedHex;
    private PlayerController player;

    public void Init(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void UpdateCurrentHex(GridHex selectedHex)
    {
        this.selectedHex = selectedHex;
    }

    public void HandleButtons()
    {
        if (selectedHex == null || player == null) return;

        HideAllButtons();

        bool isCurrentHex = selectedHex == player.CurrentHex;
        bool inMoveRange = player.HexesInMoveRadius.Contains(selectedHex);

        // Move — not current hex and in range
        if (!isCurrentHex && inMoveRange)
            MoveButton.gameObject.SetActive(true);

        // No planet — nothing else to show
        if (selectedHex.Occupant is not PlanetData planet) return;

        // Has planet — always show info
        InfoButton.gameObject.SetActive(true);

        // Not on current hex — only info
        if (!isCurrentHex) return;

        bool isOwnedByMe = planet.FactionType == player.Faction;
        bool isOwnedByEnemy = planet.FactionType != FactionType.Nothing && !isOwnedByMe;
        bool isUninhabited = planet.FactionType == FactionType.Nothing;

        if (isOwnedByMe)
        {
            bool hasStructure = planet.Structures.Count > 0;
            BuildStructureButton.gameObject.SetActive(!hasStructure);
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

        hexText.text = $"Hex {selectedHex.GridPosition.x},{selectedHex.GridPosition.y}";
  
        if (selectedHex.Occupant != null)
        {
            if (selectedHex.Occupant is PlanetData planet)
            {
                hexOccupantText.text = planet.PlanetName;
            }
        }
        else
        {
            hexOccupantText.text = "";
        }

    }

    protected override void OnShow()
    {
        Debug.Log(player.HexesInMoveRadius.Count);
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
        BuildStructureButton.gameObject.SetActive(false);
    }
}
