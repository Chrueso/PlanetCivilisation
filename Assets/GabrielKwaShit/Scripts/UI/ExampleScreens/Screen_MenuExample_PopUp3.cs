using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ExampleQuestion
{
    public string question;
    public string answer1;
    public string answer2;
}

public class Screen_MenuExample_PopUp3 : ScreenBase
{
    public string Answer => lastAnswer;

    private string lastAnswer = "NO ANSWER";

    [SerializeField] private TextMeshProUGUI questionLabel;
    [SerializeField] private Button answer1Button;
    [SerializeField] private Button answer2Button;

    private ExampleQuestion cachedQuestion;

    private void Start()
    {
        // Cannot use lambda expression because cachedQuestion can change via SetData(...)
        answer1Button.onClick.AddListener(OnClickAnswer1Button);
        answer2Button.onClick.AddListener(OnClickAnswer2Button);
    }

    public void SetData(ExampleQuestion question)
    {
        cachedQuestion = question;
    }

    protected override void OnShow()
    {
        questionLabel.text = cachedQuestion.question;
        answer1Button.GetComponentInChildren<TextMeshProUGUI>().text = cachedQuestion.answer1;
        answer2Button.GetComponentInChildren<TextMeshProUGUI>().text = cachedQuestion.answer2;
    }

    private void OnClickAnswer1Button()
    {
        lastAnswer = cachedQuestion.answer1;
        GameScreenManager.Pop();
    }

    private void OnClickAnswer2Button()
    {
        lastAnswer = cachedQuestion.answer2;
        GameScreenManager.Pop();
    }

    // Force an answer! Cannot back out!
    public override bool ShouldHonorBackButton()
    {
        return false;
    }

    public void ResetAnswer()
    {
        lastAnswer = "NO ANSWER";
    }
}