using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen_MenuExample_PopUp2 : ScreenBase
{
    [SerializeField] private TextMeshProUGUI label;

    [SerializeField] private Screen_MenuExample_PopUp3 popUp3;
    [SerializeField] private Button getAnswerButton;
    [SerializeField] private Button backButton;

    private void Start()
    {
        getAnswerButton.onClick.AddListener(() =>
        {
            ExampleQuestion question = new()
            {
                question = "Which is cuter, cat or dog?",
                answer1 = "Cat",
                answer2 = "Dog"
            };

            popUp3.SetData(question);
            GameScreenManager.Push(popUp3);
        });
        backButton.onClick.AddListener(() =>
        {
            GameScreenManager.Pop();
        });
    }

    // OnShow called before Show() happens
    // This is where you want to prepopulate the screen with data.
    protected override void OnShow()
    {
        popUp3.ResetAnswer();
        label.text = popUp3.Answer;

        // No need to call base.OnShow() because it is empty
    }

    // Focus() is called when this screen gains focus
    // happens at the end of Show()
    // or after screen on top of this screen being popped.
    public override void Focus()
    {
        label.text = popUp3.Answer;

        // [IMPORTANT] call base.Focus method before exiting
        base.Focus();
    }

    // Can also override Unfocus()
    // Unfocus() is called before another screen shows on top.
    // It is not called at the start/end of Hide()
    public override void Unfocus()
    {
        // [IMPORTANT] call base.Unfocus method before exiting
        base.Unfocus();
    }
}