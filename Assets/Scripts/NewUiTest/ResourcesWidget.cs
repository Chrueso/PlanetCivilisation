using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesWidget : MonoBehaviour, IHoverable
{
    [SerializeField] private Image icon;
    [SerializeField] public TMP_Text amountText;
    [SerializeField] private GameObject hoverVisual;

    public void OnHover()
    {
        //do something later
        //not for me pls
    }
}
