using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.DefaultInputActions;

public class UINavigationManager : Singleton<UINavigationManager>
{
    public enum UIState
    {
        BaseUI,
        FriendlySheet,
        EnemySheet,
        UnknownSheet,
        TechPanel,
        SciencePanel,
        DiplomacyPanel,
        AttackPanel,
        TradePanel
    }

    [Header("Panels")]
    [SerializeField] private GameObject friendlyPlanetSheet;
    [SerializeField] private GameObject enemyPlanetSheet;
    [SerializeField] private GameObject unknownPlanetSheet;
    [SerializeField] private GameObject techPanel;
    [SerializeField] private GameObject sciencePanel;
    [SerializeField] private GameObject diplomacyPanel;
    [SerializeField] private GameObject attackPanel;
    [SerializeField] private GameObject tradePanel;

    [Header("Base UI Elements")]
    [SerializeField] private GameObject bottomPanel;
    [SerializeField] private GameObject topResourceBar;
    [SerializeField] private GameObject minimap;

    [Header("Other")]
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private GameObject buildStructButton;
    [SerializeField] private RectTransform homeShipMoverButton;
    [SerializeField] private TextMeshProUGUI[] shipDisplay;
    [SerializeField] private TextMeshProUGUI[] shipOwned;
    [SerializeField] private Button exploreBtn;
    [SerializeField] private Button tradeBtn;
    [SerializeField] private TextMeshProUGUI[] planetName;
    [SerializeField] private TextMeshProUGUI[] planetFactions;
    [SerializeField] private TextMeshProUGUI enemyAffection;
    [SerializeField] private TextMeshProUGUI planetStructure;
    [SerializeField] private TextMeshProUGUI planetStatonedShips;
    [SerializeField] private TextMeshProUGUI enemyAffection2;

    public UIState CurrentState { get; private set; } = UIState.BaseUI;

    private UIState parentSheet;
    private PlanetData currentPlanet;
    private bool scienceBuilt;
    private bool attackBuilt;

    private void Start()
    {
        ApplySafeArea();
        AdjustBaseUIForSafeArea();
        AutoWireButtons();
        SetState(UIState.BaseUI);
        exploreBtn.onClick.AddListener(ExplorePlanet);
        tradeBtn.onClick.AddListener(IncreasePlanetAffection);
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
        if (canvas == null) canvas = FindFirstObjectByType<Canvas>();
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
        GameObject[] sheets = { friendlyPlanetSheet, enemyPlanetSheet, unknownPlanetSheet, bottomPanel };
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
                else if (n.Contains("Trade")) btn.onClick.AddListener(OpenTradePanel);
            }
        }

        GameObject[] overlays = { techPanel, sciencePanel, diplomacyPanel, attackPanel, tradePanel };
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

    private void UpdatePlanetNames(string name)
    {
        foreach (var text in planetName)
        {
            text.text = name;
        }
    }

    private void UpdateFactionName(FactionType faction)
    {
        foreach (var text in planetFactions)
        {
            text.text = $"FACTION: {faction}";
        }
    }

    public void UpdateUnfriendlyUI()
    {
        enemyAffection.text = $"AFFECTION\n{currentPlanet.Affection[FactionType.Human]}";
        enemyAffection2.text = $"{currentPlanet.Affection[FactionType.Human]}";
    }

    public void UpdateFriendlyUI()
    {
        if (currentPlanet.Structures.Count>0)
            planetStructure.text = $"STRUCTURE\n{currentPlanet.Structures[0]}";
        planetStatonedShips.text = $"SHIPS\n{HardcodeReference.Instance.ScoutShip.name}:{currentPlanet.StationedShips[HardcodeReference.Instance.ScoutShip]}" +
            $"\n{HardcodeReference.Instance.AttackShip.name}:{currentPlanet.StationedShips[HardcodeReference.Instance.AttackShip]}" +
            $"\n{HardcodeReference.Instance.WorkerShip.name}:{currentPlanet.StationedShips[HardcodeReference.Instance.WorkerShip]}";
    }

    public void ShowFriendlyPlanetSheet(PlanetData planet)
    {
        buildStructButton.SetActive(true);
        currentPlanet = planet;
        UpdatePlanetNames(planet.PlanetName);
        UpdateFactionName(planet.FactionType);
        UpdateFriendlyUI();
        SetState(UIState.FriendlySheet);
    }

    public void ShowEnemyPlanetSheet(PlanetData planet)
    {
        currentPlanet = planet;
        UpdatePlanetNames(planet.PlanetName);
        UpdateFactionName(planet.FactionType);
        UpdateUnfriendlyUI();
        SetState(UIState.EnemySheet);
    }

    public void ShowUnknownPlanetSheet(PlanetData planet)
    {
        currentPlanet = planet;
        UpdatePlanetNames(planet.PlanetName);
        SetState(UIState.UnknownSheet);
    }

    private void ExplorePlanet()
    {
        print(currentPlanet.FactionType);
        GameManager.Instance.Player.AddPlanetDiscovery(currentPlanet);
        if (!GameManager.Instance.Player.OwnedPlanets.Contains(currentPlanet))
        {
            if (currentPlanet.FactionType == FactionType.Human || currentPlanet.FactionType == FactionType.Nothing)
            {
                GameManager.Instance.Player.AddOwnedPlanets(currentPlanet);
                currentPlanet.SetFaction(FactionType.Human);
                EventsHandler.Instance.RunSimulationScreen("EXPLORATION HAPPENING", "YOU ARE SCANNING THIS PLANET", "EXPLORATION OUTCOME", "YOU COLONISED THIS PLANET");
            } else
            {
                EventsHandler.Instance.RunSimulationScreen("EXPLORATION HAPPENING", "YOU ARE SCANNING THIS PLANET", "EXPLORATION OUTCOME", "YOU FOUND THIS COLONISED PLANET");
            }
        }
        //PlayerInteractionController.Instance.inPlanet = false;
        
        DismissAllSheets();
    }

    public void DismissAllSheets()
    {
        currentPlanet = null;
        SetState(UIState.BaseUI);
        PlayerInteractionController.Instance.inPlanet = false;
        CameraController.Instance.Enable();
    }

    public void OpenTradePanel()
    {
        parentSheet = CurrentState;
        SetState(UIState.TradePanel);
    }

    private void IncreasePlanetAffection()
    {
        if (!TurnManager.Instance.currentFaction.DecreaseTurn(1)) return;
        currentPlanet.RaiseAffection(FactionType.Human, 100);
        GameManager.Instance.Player.TakeResource(ResourceType.Rations);
        GameManager.Instance.Player.GainResource(ResourceType.Metals);
        UpdateUnfriendlyUI();
        TurnManager.Instance.UpdateResourceVisuals();
    }

    public void OpenTechPanel()
    {
        parentSheet = CurrentState;
        SetState(UIState.TechPanel);
        foreach (var text in shipDisplay)
        {
            text.text = "0";
        }
        shipOwned[0].text = $"OWNED: {GameManager.Instance.Player.Ships[HardcodeReference.Instance.WorkerShip]}"; 
        shipOwned[1].text = $"OWNED: {GameManager.Instance.Player.Ships[HardcodeReference.Instance.AttackShip]}"; 
        shipOwned[2].text = $"OWNED: {GameManager.Instance.Player.Ships[HardcodeReference.Instance.ScoutShip]}"; 

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
        UpdatePlanetNames(currentPlanet.PlanetName);
        UpdateFactionName(currentPlanet.FactionType);
    }

    public void OpenAttackPanel()
    {
        //parentSheet = CurrentState;
        bool canAttack = TurnManager.Instance.currentFaction.DecreaseTurn(2);
        print(canAttack);
        if (!canAttack) return;
        SetState(UIState.AttackPanel);
        BattleResult result = BattleManager.Instance.Battle(GameManager.Instance.Player.Ships, new Dictionary<ShipTypeSO, int>() { {HardcodeReference.Instance.AttackShip,1 } } );
        EventsHandler.Instance.RunSimulationScreen("ATTACK HAPPENING", $"YOU ARE ATTACKING {currentPlanet.PlanetName}", "ATTACK OUTCOME", $"WIN: {result.AttackerWon}");
        if (result.AttackerWon == true)
        {
            currentPlanet.SetFaction(FactionType.Human);
            GameManager.Instance.Player.AddOwnedPlanets(currentPlanet);
            CameraController.Instance.Enable();
            DismissAllSheets();
        }
    }

    public void MoveHomeShipButton(Vector3 screenPos)
    {
        homeShipMoverButton.position = screenPos + new Vector3(0f, 10f, 1f);
    }
    
    public void SetHomeShipButton(bool active)
    {
        homeShipMoverButton.gameObject.SetActive(active);   
    }


    public void BackFromOverlay()
    {
        print(parentSheet);
        if (parentSheet == UIState.FriendlySheet || parentSheet == UIState.EnemySheet)
            SetState(parentSheet); 
        else
            SetState(UIState.BaseUI);
    }

    private void SetState(UIState newState)
    {
        CurrentState = newState;

        bool showBase = newState == UIState.BaseUI || newState == UIState.AttackPanel;
        if (bottomPanel) bottomPanel.SetActive(showBase);
        if (topResourceBar) topResourceBar.SetActive(true);
        //if (minimap) minimap.SetActive(showBase);
        if (nextTurnButton) nextTurnButton.gameObject.SetActive(showBase);
        buildStructButton.SetActive(true);
        if (currentPlanet != null && currentPlanet.Structures.Count > 0) buildStructButton.SetActive(false);
        

        if (friendlyPlanetSheet) friendlyPlanetSheet.SetActive(newState == UIState.FriendlySheet);
        if (enemyPlanetSheet) enemyPlanetSheet.SetActive(newState == UIState.EnemySheet || newState == UIState.AttackPanel);
        if (unknownPlanetSheet) unknownPlanetSheet.SetActive(newState == UIState.UnknownSheet);
        if (tradePanel) tradePanel.SetActive(newState == UIState.TradePanel);
        if (techPanel) techPanel.SetActive(newState == UIState.TechPanel);
        if (sciencePanel) sciencePanel.SetActive(newState == UIState.SciencePanel);
        if (diplomacyPanel) diplomacyPanel.SetActive(newState == UIState.DiplomacyPanel);

        // Build content on first open (panel must be active first)
        if (newState == UIState.SciencePanel && !scienceBuilt)
        {
            return;
            scienceBuilt = true;
            // Force layout to settle after SafeArea anchors applied
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(sciencePanel.GetComponent<RectTransform>());
            BuildScienceContent();
        }
        if (newState == UIState.AttackPanel && !attackBuilt)
        {
            return;
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
