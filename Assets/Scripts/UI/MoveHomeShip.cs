using UnityEngine;
using UnityEngine.UI;

public class MoveHomeShip : MonoBehaviour
{
    [SerializeField] private Button moveHomeShipBtn;

    private void OnEnable()
    {
        moveHomeShipBtn.onClick.AddListener(MoveHomeShipHere);
    }

    private void MoveHomeShipHere()
    {
        /*
        if (!TurnManager.Instance.currentFaction.DecreaseTurn(1)) return;
        UINavigationManager.Instance.SetHomeShipButton(false);
        GameManager.Instance.Player.MoveToHex(PlayerInteractionController.Instance.CurrentGridHex);
        */
    }
}
