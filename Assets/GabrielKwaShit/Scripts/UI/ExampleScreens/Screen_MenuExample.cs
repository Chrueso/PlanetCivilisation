using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Screen_MenuExample : ScreenBase
{
    [SerializeField] private Screen_MenuExample_PopUp1 settingsScreen;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button playButton;

    private void Start()
    {
        settingsButton.onClick.AddListener(() =>
        {
            GameScreenManager.Push(settingsScreen);
        });

        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("ExampleScene2");
        });
    }
}