using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetView))]
public class PlanetVisualEditor : Editor
{
    private PlanetView planetVisual;

    private void OnEnable()
    {
        planetVisual = (PlanetView)target;
    }

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();

            if (check.changed)
            {
                if (planetVisual.AutoUpdate) planetVisual.UpdatePlanetVisual();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planetVisual.UpdatePlanetVisual();
        }
    }
}

