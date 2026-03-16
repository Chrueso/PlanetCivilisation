using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Screen_Scene2Example : ScreenBase
{
    [SerializeField] private Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("ExampleScene1");
        });
    }
}