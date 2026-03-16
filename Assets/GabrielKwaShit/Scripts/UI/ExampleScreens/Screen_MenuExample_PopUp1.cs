using UnityEngine;
using UnityEngine.UI;

public class Screen_MenuExample_PopUp1 : ScreenBase
{
    [SerializeField] private Screen_MenuExample_PopUp2 popUp2;
    [SerializeField] private Button popup2Button;
    [SerializeField] private Button backButton;

    private void Start()
    {
        popup2Button.onClick.AddListener(() =>
        {
            GameScreenManager.Push(popUp2);
        });
        backButton.onClick.AddListener(() =>
        {
            GameScreenManager.Pop();
        });
    }
}