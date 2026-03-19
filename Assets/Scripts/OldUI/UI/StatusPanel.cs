using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : Singleton<StatusPanel>
{
    [SerializeField] private Image statusPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;

    public void ShowText(string title, string text)
    {
        statusPanel.gameObject.SetActive(true);
        titleText.text = title;
        bodyText.text = text;
        StartCoroutine(Count());
    }

    private IEnumerator Count()
    {
        yield return new WaitForSeconds(2f);
        statusPanel.gameObject.SetActive(false);
    }
}
