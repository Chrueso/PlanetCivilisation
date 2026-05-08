using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonYay : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private GameObject credit;
    [SerializeField] private Button closeCredit;
    [SerializeField] private AudioLibrary audioLib;
    [SerializeField] private AudioSystem audioSystem;


    private void OnEnable()
    {
        startButton.onClick.AddListener(Yay);
        creditButton.onClick.AddListener(OpenCredits);
        closeCredit.onClick.AddListener(CloseCredits);
        audioSystem.PlayMusic(audioLib.mainMenuMusic);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(Yay);
        creditButton.onClick.RemoveListener(OpenCredits);
        closeCredit.onClick.RemoveListener(CloseCredits);
    }

    private void Yay()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    private void OpenCredits()
    {
        credit.SetActive(true);
    }

    private void CloseCredits()
    {
        credit.SetActive(false);
    }

}
