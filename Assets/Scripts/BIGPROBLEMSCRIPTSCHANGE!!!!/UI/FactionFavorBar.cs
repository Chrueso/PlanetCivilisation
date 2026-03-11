using UnityEngine;
using UnityEngine.UI;

public class FactionFavorBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform fillBar;

    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float fillAmount = 0.4f;
    [SerializeField] private Color friendlyColor = new Color(1f, 0.078f, 0.576f, 1f); // hot pink
    [SerializeField] private Color hostileColor = new Color(0.8f, 0.2f, 0.2f, 1f);    // red
    [SerializeField] private Color neutralColor = new Color(0.5f, 0.5f, 0.5f, 1f);     // grey

    private Image fillImage;

    private void Awake()
    {
        if (fillBar != null)
            fillImage = fillBar.GetComponent<Image>();
    }

    public float FillAmount
    {
        get => fillAmount;
        set
        {
            fillAmount = Mathf.Clamp01(value);
            UpdateBar();
        }
    }

    public void SetFavor(float normalized)
    {
        FillAmount = normalized;
    }

    private void UpdateBar()
    {
        if (fillBar == null) return;

        fillBar.anchorMin = new Vector2(0f, 0f);
        fillBar.anchorMax = new Vector2(fillAmount, 1f);
        fillBar.offsetMin = Vector2.zero;
        fillBar.offsetMax = Vector2.zero;

        if (fillImage != null)
        {
            if (fillAmount < 0.33f)
                fillImage.color = Color.Lerp(hostileColor, neutralColor, fillAmount / 0.33f);
            else
                fillImage.color = Color.Lerp(neutralColor, friendlyColor, (fillAmount - 0.33f) / 0.67f);
        }
    }

    private void OnValidate()
    {
        UpdateBar();
    }
}
