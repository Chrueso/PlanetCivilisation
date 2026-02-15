using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetVisual))]
public class PlanetVisualEditor : Editor
{
    private PlanetVisual planetVisual;

    private void OnEnable()
    {
        planetVisual = (PlanetVisual)target;
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

