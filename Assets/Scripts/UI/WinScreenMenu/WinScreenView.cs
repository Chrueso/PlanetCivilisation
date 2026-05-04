using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenView : ScreenBase
{
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI InfoText;
    [SerializeField] private Button ReturnButton;

    private TurnManager turnManager;
    public void Init(TurnManager turnManager)
    {
        this.turnManager = turnManager;
    }

    private void OnEnable()
    {
        ReturnButton.onClick.AddListener(OnReturnButtonClicked);    
    }

    private void OnDisable()
    {
        ReturnButton.onClick.RemoveListener(OnReturnButtonClicked);
    }

    public void ShowWinner(IEntityController entity)
    {
        EntityModel winnerEntity = entity.GetModel();
        TitleText.text = $"WINNER - {winnerEntity.FactionType}";
        InfoText.text = $"PLANETS OWNED:{winnerEntity.OwnedPlanets.Count}\r\n" +
            $"TOTAL SHIPS:{winnerEntity.GetShipTotal()}\r\n" +
            $"TOTAL RESOURCES:{winnerEntity.Resources[ResourceType.Metals] + winnerEntity.Resources[ResourceType.Rations]}\r\n" +
            $"TURNS PLAYED:{turnManager.CurrentTurn}\r\n";
    }

    protected override void OnShow()
    {
        
    }

    private void OnReturnButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
