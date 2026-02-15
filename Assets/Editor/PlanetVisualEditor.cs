//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlanetVisual))]
//public class PlanetVisualEditor : Editor
//{
//    private PlanetVisual planetVisual;
//    private Editor presetEditor;

//    public override void OnInspectorGUI()
//    {
//        using (var check = new EditorGUI.ChangeCheckScope())
//        {
//            base.OnInspectorGUI();

//            if (check.changed)
//            {
//                planetVisual.GeneratePlanetVisual();
//            }
//        }

//        if (GUILayout.Button("Generate Planet"))
//        {
//            planetVisual.GeneratePlanetVisual();
//        }

//        DrawSettingsEditor(planetVisual.ShapeSettings, planetVisual.OnPresetUpdated, ref planetVisual.ShapeSettingsFoldout, ref presetEditor);
//    }


//    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
//    {
//        if (settings == null) return;

//        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

//        using var check = new EditorGUI.ChangeCheckScope();

//        if (foldout)
//        {
//            CreateCachedEditor(settings, null, ref editor); // only creates new editor when needed
//            editor.OnInspectorGUI();

//            if (check.changed)
//            {
//                if (onSettingsUpdated != null)
//                {
//                    onSettingsUpdated();
//                }
//            }
//        }

//    }

//    private void OnEnable()
//    {
//        planetVisual = (PlanetVisual)target;
//    }
//}

