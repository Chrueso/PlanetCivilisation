using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonYay : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private AudioLibrary audioLib;
    [SerializeField] private AudioSystem audioSystem;


    private void OnEnable()
    {
        startButton.onClick.AddListener(Yay);
        audioSystem.PlayMusic(audioLib.mainMenuMusic);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(Yay);
    }

    private void Yay()
    {
        SceneManager.LoadScene("MainGameScene");
    }
}
