using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "UITheme", menuName = "PlanetCiv/UI Theme")]
public class UITheme : ScriptableObject
{
    [Header("Fonts")]
    public TMP_FontAsset logoFont;      // Rubik Glitch — game title only
    public TMP_FontAsset titleFont;     // Zen Dots — buttons, headings
    public TMP_FontAsset bodyFont;      // Digital-7 / Share Tech Mono — labels, values

    [Header("Font Sizes")]
    public float logoSize = 42f;
    public float headingSize = 22f;
    public float buttonSize = 16f;
    public float bodySize = 18f;
    public float captionSize = 14f;

    [Header("Colors — Figma palette")]
    public Color cyan = new Color(0f, 0.898f, 1f);           // #00E5FF
    public Color magenta = new Color(1f, 0f, 0.502f);        // #FF0080
    public Color green = new Color(0f, 1f, 0.533f);          // #00FF88
    public Color gold = new Color(1f, 0.843f, 0f);           // #FFD700
    public Color purpleBg = new Color(0.102f, 0.039f, 0.180f);    // #1A0A2E
    public Color purpleMid = new Color(0.176f, 0.106f, 0.306f);   // #2D1B4E
    public Color purpleLight = new Color(0.239f, 0.169f, 0.369f); // #3D2B5E
    public Color dark = new Color(0.051f, 0.008f, 0.129f);        // #0D0221
    public Color textColor = new Color(0.878f, 0.902f, 0.941f);   // #E0E6F0
    public Color textDim = new Color(0.541f, 0.498f, 0.667f);     // #8A7FAA

    [Header("Panel")]
    public Color panelBg = new Color(0.078f, 0.039f, 0.196f, 0.85f);
    public Color panelBorder = new Color(0f, 0.898f, 1f, 0.25f);
    public Color panelBorderStrong = new Color(0f, 0.898f, 1f, 0.5f);

    [Header("Button")]
    public Color buttonBg = new Color(0f, 0.898f, 1f, 0.08f);
    public Color buttonBorder = new Color(0f, 0.898f, 1f, 0.5f);
    public float buttonLetterSpacing = 8f;
}
