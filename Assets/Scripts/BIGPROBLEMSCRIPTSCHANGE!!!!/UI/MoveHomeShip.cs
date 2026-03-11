using UnityEngine;
using UnityEngine.UI;

public class MoveHomeShip : MonoBehaviour
{
    [SerializeField] UINavigationManager uINavigationManager;
    [SerializeField] private Button moveHomeShipBtn;

    private void OnEnable()
    {
        moveHomeShipBtn.onClick.AddListener(MoveHomeShipHere);
    }

    private void MoveHomeShipHere()
    {
        if (!TurnManager.Instance.currentFaction.DecreaseActionPoint(1)) return;
        uINavigationManager.SetHomeShipButton(false);
        GameManager.Instance.Player.MoveToHex(PlayerInteractionController.Instance.CurrentGridHex);
        
    }
}
