using TMPro;
using UnityEngine;

public class InfoElements : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Value;

    public void ChangeText(string title, string value)
    {
        Title.text = title;
        Value.text = value;
    }
    public void Omit()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
