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
        UINavigationManager.Instance.SetHomeShipButton(false);
    }
}
