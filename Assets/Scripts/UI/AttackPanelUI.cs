using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackPanelUI : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color cardColor = new Color(0.18f, 0.18f, 0.28f, 1f);
    [SerializeField] private Color accentColor = new Color(0.85f, 0.25f, 0.25f, 1f);
    [SerializeField] private Color subtleText = new Color(0.6f, 0.6f, 0.7f, 1f);

    private RectTransform contentRect;

    public void Init()
    {
        contentRect = GetComponent<RectTransform>();
        Canvas.ForceUpdateCanvases();

        float visibleHeight = 1670f;
        if (contentRect.parent != null)
        {
            var parentRect = contentRect.parent as RectTransform;
            if (parentRect != null && parentRect.rect.height > 0f)
                visibleHeight = parentRect.rect.height;
        }

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, visibleHeight);
        BuildPlaceholderUI(visibleHeight);
    }

    private void BuildPlaceholderUI(float visibleHeight)
    {
        float fontSize = 36f;
        float headerFontSize = 40f;
        float btnFontSize = 44f;
        float cardH = 130f;
        float cardGap = 20f;
        float rowH = 90f;
        float rowGap = 20f;
        float btnH = 120f;
        float btnGap = 30f;
        float sectionH = 60f;
        float sectionGap = 20f;
        float groupGap = 60f;
        float bottomPad = 160f;

        // --- Top content (from top down) ---
        float y = -40f;

        y = CreateSectionLabel("SELECT FLEET", y, sectionH, headerFontSize);
        y -= sectionGap;
        y = CreateShipCard("Fighter Squadron", "x12", y, cardH, fontSize);
        y -= cardGap;
        y = CreateShipCard("Destroyer", "x3", y, cardH, fontSize);
        y -= cardGap;
        y = CreateShipCard("Carrier", "x1", y, cardH, fontSize);

        y -= groupGap;
        y = CreateSectionLabel("TARGET INFO", y, sectionH, headerFontSize);
        y -= sectionGap;
        y = CreateInfoRow("Planet", "Unknown", y, rowH, fontSize);
        y -= rowGap;
        y = CreateInfoRow("Faction", "Enemy", y, rowH, fontSize);
        y -= rowGap;
        y = CreateInfoRow("Defense", "Medium", y, rowH, fontSize);

        // --- Buttons anchored to bottom ---
        float btnBottom = -visibleHeight + bottomPad;
        float retreatY = btnBottom;
        float launchY = retreatY + btnH + btnGap;

        CreateActionButton("LAUNCH ATTACK", accentColor, launchY, btnH, btnFontSize);
        CreateActionButton("RETREAT", cardColor, retreatY, btnH, btnFontSize);
    }

    private float CreateSectionLabel(string text, float yPos, float height, float fontSize)
    {
        GameObject go = new GameObject("Section", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(contentRect, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, yPos);
        rt.sizeDelta = new Vector2(-80, height);

        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = subtleText;
        tmp.alignment = TextAlignmentOptions.Left;

        return yPos - height;
    }

    private float CreateShipCard(string shipName, string count, float yPos, float height, float fontSize)
    {
        GameObject card = new GameObject("ShipCard", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        card.transform.SetParent(contentRect, false);

        RectTransform rt = card.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, yPos);
        rt.sizeDelta = new Vector2(-80, height);

        card.GetComponent<Image>().color = cardColor;

        float iconSize = height * 0.6f;
        GameObject icon = new GameObject("Icon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        icon.transform.SetParent(card.transform, false);
        RectTransform iconRT = icon.GetComponent<RectTransform>();
        iconRT.anchorMin = new Vector2(0, 0.5f);
        iconRT.anchorMax = new Vector2(0, 0.5f);
        iconRT.anchoredPosition = new Vector2(70, 0);
        iconRT.sizeDelta = new Vector2(iconSize, iconSize);
        icon.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.45f, 1f);

        GameObject nameGO = new GameObject("Name", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        nameGO.transform.SetParent(card.transform, false);
        RectTransform nameRT = nameGO.GetComponent<RectTransform>();
        nameRT.anchorMin = Vector2.zero;
        nameRT.anchorMax = Vector2.one;
        nameRT.offsetMin = new Vector2(130, 10);
        nameRT.offsetMax = new Vector2(-120, -10);
        TextMeshProUGUI nameTMP = nameGO.GetComponent<TextMeshProUGUI>();
        nameTMP.text = shipName;
        nameTMP.fontSize = fontSize;
        nameTMP.color = Color.white;
        nameTMP.alignment = TextAlignmentOptions.Left;

        GameObject countGO = new GameObject("Count", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        countGO.transform.SetParent(card.transform, false);
        RectTransform countRT = countGO.GetComponent<RectTransform>();
        countRT.anchorMin = new Vector2(1, 0);
        countRT.anchorMax = new Vector2(1, 1);
        countRT.anchoredPosition = new Vector2(-60, 0);
        countRT.sizeDelta = new Vector2(100, 0);
        TextMeshProUGUI countTMP = countGO.GetComponent<TextMeshProUGUI>();
        countTMP.text = count;
        countTMP.fontSize = fontSize + 4;
        countTMP.color = subtleText;
        countTMP.alignment = TextAlignmentOptions.Center;

        return yPos - height;
    }

    private float CreateInfoRow(string label, string value, float yPos, float height, float fontSize)
    {
        GameObject row = new GameObject("InfoRow", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        row.transform.SetParent(contentRect, false);

        RectTransform rt = row.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, yPos);
        rt.sizeDelta = new Vector2(-80, height);
        row.GetComponent<Image>().color = new Color(cardColor.r, cardColor.g, cardColor.b, 0.5f);

        GameObject labelGO = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        labelGO.transform.SetParent(row.transform, false);
        RectTransform labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = new Vector2(0.5f, 1);
        labelRT.offsetMin = new Vector2(30, 0);
        labelRT.offsetMax = Vector2.zero;
        TextMeshProUGUI labelTMP = labelGO.GetComponent<TextMeshProUGUI>();
        labelTMP.text = label;
        labelTMP.fontSize = fontSize;
        labelTMP.color = subtleText;
        labelTMP.alignment = TextAlignmentOptions.Left;

        GameObject valGO = new GameObject("Value", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        valGO.transform.SetParent(row.transform, false);
        RectTransform valRT = valGO.GetComponent<RectTransform>();
        valRT.anchorMin = new Vector2(0.5f, 0);
        valRT.anchorMax = Vector2.one;
        valRT.offsetMin = Vector2.zero;
        valRT.offsetMax = new Vector2(-30, 0);
        TextMeshProUGUI valTMP = valGO.GetComponent<TextMeshProUGUI>();
        valTMP.text = value;
        valTMP.fontSize = fontSize;
        valTMP.color = Color.white;
        valTMP.alignment = TextAlignmentOptions.Right;

        return yPos - height;
    }

    private void CreateActionButton(string text, Color bgColor, float yPos, float height, float fontSize)
    {
        GameObject btnGO = new GameObject("ActionBtn", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btnGO.transform.SetParent(contentRect, false);

        RectTransform rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, yPos);
        rt.sizeDelta = new Vector2(-100, height);

        btnGO.GetComponent<Image>().color = bgColor;

        GameObject labelGO = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        labelGO.transform.SetParent(btnGO.transform, false);
        RectTransform labelRT = labelGO.GetComponent<RectTransform>();
        labelRT.anchorMin = Vector2.zero;
        labelRT.anchorMax = Vector2.one;
        labelRT.offsetMin = Vector2.zero;
        labelRT.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = labelGO.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
    }
}
