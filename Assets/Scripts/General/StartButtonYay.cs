using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonYay : MonoBehaviour
{
    [SerializeField] private Button startButton;


    private void OnEnable()
    {
        startButton.onClick.AddListener(Yay);
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
