using UnityEngine;
using UnityEngine.UI;

public class UINavigationManager : Singleton<UINavigationManager>
{
    public enum UIState
    {
        BaseUI,
        FriendlySheet,
        EnemySheet,
        TechPanel,
        SciencePanel,
        DiplomacyPanel,
        AttackPanel
    }

    [Header("Panels")]
    [SerializeField] private GameObject friendlyPlanetSheet;
    [SerializeField] private GameObject enemyPlanetSheet;
    [SerializeField] private GameObject techPanel;
    [SerializeField] private GameObject sciencePanel;
    [SerializeField] private GameObject diplomacyPanel;
    [SerializeField] private GameObject attackPanel;

    [Header("Base UI Elements")]
    [SerializeField] private GameObject bottomPanel;
    [SerializeField] private GameObject topResourceBar;
    [SerializeField] private GameObject minimap;

    [Header("Other")]
    [SerializeField] private Button nextTurnButton;

    public UIState CurrentState { get; private set; } = UIState.BaseUI;

    private UIState parentSheet;
    private Planet currentPlanet;
    private bool scienceBuilt;
    private bool attackBuilt;

    private void Start()
    {
        ApplySafeArea();
        AdjustBaseUIForSafeArea();
        AutoWireButtons();
        SetState(UIState.BaseUI);
    }

    private void ApplySafeArea()
    {
        // For each overlay panel: keep background full-screen,
        // wrap content in a SafeArea child that respects device notches
        GameObject[] overlays = { techPanel, sciencePanel, diplomacyPanel, attackPanel };
        foreach (var panel in overlays)
            WrapPanelContentInSafeArea(panel);
    }

    private void WrapPanelContentInSafeArea(GameObject panel)
    {
        if (panel == null) return;

        // Create SafeArea child (sits inside the panel, on top of its background)
        GameObject safeAreaGO = new GameObject("SafeArea", typeof(RectTransform));
        safeAreaGO.transform.SetParent(panel.transform, false);
        RectTransform safeRT = safeAreaGO.GetComponent<RectTransform>();
        safeRT.anchorMin = Vector2.zero;
        safeRT.anchorMax = Vector2.one;
        safeRT.offsetMin = Vector2.zero;
        safeRT.offsetMax = Vector2.zero;
        safeAreaGO.AddComponent<SafeAreaAdapter>();

        // Reparent all existing panel children into the SafeArea wrapper
        var children = new System.Collections.Generic.List<Transform>();
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            Transform child = panel.transform.GetChild(i);
            if (child != safeAreaGO.transform)
                children.Add(child);
        }
        foreach (var child in children)
            child.SetParent(safeRT, false);
    }

    private void AdjustBaseUIForSafeArea()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        Canvas.ForceUpdateCanvases();
        float canvasH = canvas.GetComponent<RectTransform>().rect.height;
        if (canvasH <= 0 || Screen.height <= 0) return;

        float scaleY = canvasH / Screen.height;
        // Use same minimum as SafeAreaAdapter (3.5% of screen)
        float bottomInset = Mathf.Max(Screen.safeArea.yMin * scaleY, canvasH * 0.035f);

        if (bottomPanel)
        {
            var rt = bottomPanel.GetComponent<RectTransform>();
            rt.anchoredPosition += new Vector2(0, bottomInset);
        }
    }

    private void AutoWireButtons()
    {
        GameObject[] sheets = { friendlyPlanetSheet, enemyPlanetSheet, bottomPanel };
        foreach (var sheet in sheets)
        {
            if (sheet == null) continue;
            foreach (var btn in sheet.GetComponentsInChildren<Button>(true))
            {
                string n = btn.gameObject.name;
                if (n.Contains("Tech")) btn.onClick.AddListener(OpenTechPanel);
                else if (n.Contains("Science")) btn.onClick.AddListener(OpenSciencePanel);
                else if (n.Contains("Diplomacy") || n.Contains("Dip")) btn.onClick.AddListener(OpenDiplomacyPanel);
                else if (n.Contains("Attack")) btn.onClick.AddListener(OpenAttackPanel);
            }
        }

        GameObject[] overlays = { techPanel, sciencePanel, diplomacyPanel, attackPanel };
        foreach (var overlay in overlays)
        {
            if (overlay == null) continue;
            foreach (var btn in overlay.GetComponentsInChildren<Button>(true))
            {
                if (btn.gameObject.name.Contains("Back"))
                    btn.onClick.AddListener(BackFromOverlay);
            }
        }
    }

    public void ShowFriendlyPlanetSheet(Planet planet)
    {
        currentPlanet = planet;
        SetState(UIState.FriendlySheet);
    }

    public void ShowEnemyPlanetSheet(Planet planet)
    {
        currentPlanet = planet;
        SetState(UIState.EnemySheet);
    }

    public void DismissAllSheets()
    {
        currentPlanet = null;
        SetState(UIState.BaseUI);
    }

    public void OpenTechPanel()
    {
        parentSheet = CurrentState;
        SetState(UIState.TechPanel);
    }

    public void OpenSciencePanel()
    {
        parentSheet = CurrentState;
        SetState(UIState.SciencePanel);
    }

    public void OpenDiplomacyPanel()
    {
        parentSheet = CurrentState;
        SetState(UIState.DiplomacyPanel);
    }

    public void OpenAttackPanel()
    {
        parentSheet = CurrentState;
        SetState(UIState.AttackPanel);
    }

    public void BackFromOverlay()
    {
        if (parentSheet == UIState.FriendlySheet || parentSheet == UIState.EnemySheet)
            SetState(parentSheet);
        else
            SetState(UIState.BaseUI);
    }

    private void SetState(UIState newState)
    {
        CurrentState = newState;

        bool showBase = newState == UIState.BaseUI;
        if (bottomPanel) bottomPanel.SetActive(showBase);
        if (topResourceBar) topResourceBar.SetActive(true);
        if (minimap) minimap.SetActive(showBase);
        if (nextTurnButton) nextTurnButton.gameObject.SetActive(showBase);

        if (friendlyPlanetSheet) friendlyPlanetSheet.SetActive(newState == UIState.FriendlySheet);
        if (enemyPlanetSheet) enemyPlanetSheet.SetActive(newState == UIState.EnemySheet);
        if (techPanel) techPanel.SetActive(newState == UIState.TechPanel);
        if (sciencePanel) sciencePanel.SetActive(newState == UIState.SciencePanel);
        if (diplomacyPanel) diplomacyPanel.SetActive(newState == UIState.DiplomacyPanel);
        if (attackPanel) attackPanel.SetActive(newState == UIState.AttackPanel);

        // Build content on first open (panel must be active first)
        if (newState == UIState.SciencePanel && !scienceBuilt)
        {
            scienceBuilt = true;
            // Force layout to settle after SafeArea anchors applied
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(sciencePanel.GetComponent<RectTransform>());
            BuildScienceContent();
        }
        if (newState == UIState.AttackPanel && !attackBuilt)
        {
            attackBuilt = true;
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(attackPanel.GetComponent<RectTransform>());
            BuildAttackContent();
        }
    }

    private void BuildScienceContent()
    {
        if (!sciencePanel) return;
        var contentRT = FindChildByName(sciencePanel.transform, "ScienceContent");
        if (contentRT == null) return;
        var tree = contentRT.gameObject.AddComponent<ScienceTechTree>();
        tree.Init();
    }

    private void BuildAttackContent()
    {
        if (!attackPanel) return;
        var contentRT = FindChildByName(attackPanel.transform, "AttackContent");
        if (contentRT == null) return;
        var ui = contentRT.gameObject.AddComponent<AttackPanelUI>();
        ui.Init();
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        foreach (var t in parent.GetComponentsInChildren<Transform>(true))
        {
            if (t.gameObject.name == name)
                return t;
        }
        return null;
    }
}
