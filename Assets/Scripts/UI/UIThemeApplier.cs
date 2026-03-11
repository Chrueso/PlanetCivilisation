using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class UIThemeApplier : MonoBehaviour
{
    public enum TextRole { Logo, Heading, Button, Body, Caption }
    public enum ColorRole { Cyan, Magenta, Green, Gold, Text, TextDim, PanelBg, PanelBorder, ButtonBg, ButtonBorder }

    [Header("Theme Asset")]
    public UITheme theme;

    [Header("Text (if TextMeshProUGUI present)")]
    public bool applyText = true;
    public TextRole textRole = TextRole.Body;

    [Header("Color (for Image or Text)")]
    public bool applyColor = true;
    public ColorRole colorRole = ColorRole.Text;

    private void OnEnable() => Apply();

    private void OnValidate()
    {
#if UNITY_EDITOR
        // editor needs delay or it breaks
        EditorApplication.delayCall += () =>
        {
            if (this != null) Apply();
        };
#else
        Apply();
#endif
    }

    public void Apply()
    {
        if (theme == null) return;

        var tmp = GetComponent<TextMeshProUGUI>();
        if (tmp != null && applyText)
        {
            // font + size from role
            switch (textRole)
            {
                case TextRole.Logo:
                    tmp.font = theme.logoFont;
                    tmp.fontSize = theme.logoSize;
                    break;
                case TextRole.Heading:
                    tmp.font = theme.titleFont;
                    tmp.fontSize = theme.headingSize;
                    tmp.characterSpacing = 4f;
                    break;
                case TextRole.Button:
                    tmp.font = theme.titleFont;
                    tmp.fontSize = theme.buttonSize;
                    tmp.characterSpacing = theme.buttonLetterSpacing;
                    break;
                case TextRole.Body:
                    tmp.font = theme.bodyFont;
                    tmp.fontSize = theme.bodySize;
                    break;
                case TextRole.Caption:
                    tmp.font = theme.bodyFont;
                    tmp.fontSize = theme.captionSize;
                    break;
            }

            // colorRole overrides default if applyColor is on
            if (applyColor)
                tmp.color = GetColor(colorRole);
            else
                tmp.color = GetDefaultTextColor(textRole);
        }

        // image color
        if (applyColor)
        {
            var img = GetComponent<Image>();
            if (img != null) img.color = GetColor(colorRole);

            if (tmp != null && !applyText) tmp.color = GetColor(colorRole);
        }
    }

    private Color GetDefaultTextColor(TextRole role)
    {
        return role switch
        {
            TextRole.Logo => theme.cyan,
            TextRole.Heading => theme.cyan,
            TextRole.Button => theme.cyan,
            TextRole.Body => theme.textColor,
            TextRole.Caption => theme.textDim,
            _ => theme.textColor
        };
    }

    private Color GetColor(ColorRole role)
    {
        return role switch
        {
            ColorRole.Cyan => theme.cyan,
            ColorRole.Magenta => theme.magenta,
            ColorRole.Green => theme.green,
            ColorRole.Gold => theme.gold,
            ColorRole.Text => theme.textColor,
            ColorRole.TextDim => theme.textDim,
            ColorRole.PanelBg => theme.panelBg,
            ColorRole.PanelBorder => theme.panelBorder,
            ColorRole.ButtonBg => theme.buttonBg,
            ColorRole.ButtonBorder => theme.buttonBorder,
            _ => Color.white
        };
    }
}
