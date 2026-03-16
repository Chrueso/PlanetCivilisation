using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleScreenController : MonoBehaviour
{
    private readonly List<ScreenBase> screens = new();

    // The first screen to show, MUST NOT BE NULL
    [SerializeField] private ScreenBase startingScreen;

    // If true, the starting screen will instantly be shown
    [SerializeField] private bool instantlyShowStartingScreen = false;

    private InputAction backAction;

    private void Awake()
    {
        GameScreenManager.Register(this);
        backAction = InputSystem.actions.FindAction("Back");
    }

    private void OnDestroy() => GameScreenManager.Unregister(this);

    private void Start()
    {
        Push(startingScreen, instantlyShowStartingScreen);
    }

    public void Push(ScreenBase newScreen, bool instant = false)
    {
        // unfocus current screen, if any
        // add new screen to end of collection
        // show the new screen, respecting 'instant'

        if (screens.Count > 0 && screens[^1] != null) // [^1] is last item of array
        {
            screens[^1].Unfocus();
        }

        screens.Add(newScreen);

        newScreen.Show(instant);
    }

    public void Pop(bool instant = false)
    {
        // hide current screen, respecting 'instant'
        // remove current screen from end of collection
        // focus previous screen

        if (screens.Count > 0 && screens[^1] != null) // [^1] is last item of array
        {
            screens[^1].Hide(instant);
        }

        // remove current screen
        if (screens.Count > 0) screens.RemoveAt(screens.Count - 1);

        // focus previous screen only if one exists
        if (screens.Count > 0 && screens[^1] != null)
        {
            screens[^1].Focus();
        }
    }

    private void Update()
    {
        // if Back key is pressed...
        // check if collection has more than 1 screen in it
        // if yes, check if the current screen should honor back button
        //      if yes, pop
        // otherwise, do nothing

        if (backAction.WasPerformedThisFrame() && screens.Count > 1)
        {
            if (screens[^1].ShouldHonorBackButton())
            {
                Pop(true);
            }
        }

    }

    public bool IsTopScreen(ScreenBase screen)
    {
        return screens.Count > 0 && screens[^1] == screen;
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.fontSize = 36;
        fontStyle.normal.textColor = Color.white;

        GUILayout.BeginVertical();

        GUILayout.Label("SimpleScreenManager [Editor DebugView]", fontStyle);
        GUILayout.Label("Screens:", fontStyle);
        for (int i = 0; i < screens.Count; i++)
        {
            bool isLast = i == screens.Count - 1;
            fontStyle.normal.textColor = isLast ? Color.green : Color.white;

            var screen = screens[i];
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label($"[{i}] {screen.name} {(isLast ? "<--" : "")}", fontStyle);
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

#endif
}
