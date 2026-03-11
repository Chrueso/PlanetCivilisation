using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        foreach (var btn in GetComponentsInChildren<Button>(true))
        {
            string n = btn.gameObject.name;
            if (n.Contains("Start")) btn.onClick.AddListener(StartGame);
            else if (n.Contains("Exit")) btn.onClick.AddListener(ExitGame);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
