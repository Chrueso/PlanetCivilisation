//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlanetVisual))]
//public class PlanetVisualEditor : Editor
//{
//    private PlanetVisual planetVisual;
//    private Editor shapeEditor;
//    private Editor colorEditor;

//    public override void OnInspectorGUI()
//    {
//        using (var check = new EditorGUI.ChangeCheckScope())
//        {
//            //base.OnInspectorGUI();

//            if (check.changed)
//            {   
//                planetVisual.GeneratePlanetVisual();
//            }
//        }

//        if (GUILayout.Button("Generate Planet"))
//        {
//            planetVisual.GeneratePlanetVisual();
//        }

//        DrawSettingsEditor(planetVisual.ShapeSettings, planetVisual.OnShapeSettingsUpdated, ref planetVisual.ShapeSettingsFoldout, ref shapeEditor);
//        DrawSettingsEditor(planetVisual.ColorSettings, planetVisual.OnColorSettingsUpdated, ref planetVisual.ColorSettingsFoldout, ref colorEditor);
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

//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(PlanetVisual))]
//public class PlanetVisualEditor : Editor
//{
//    private SerializedProperty resolutionProp;
//    private SerializedProperty autoUpdateProp;
//    private SerializedProperty shapeSettingsProp;
//    private SerializedProperty colorSettingsProp;

//    private void OnEnable()
//    {
//        resolutionProp = serializedObject.FindProperty("Resolution");
//        autoUpdateProp = serializedObject.FindProperty("AutoUpdate");
//        shapeSettingsProp = serializedObject.FindProperty("ShapeSettings");
//        colorSettingsProp = serializedObject.FindProperty("ColorSettings");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        EditorGUILayout.PropertyField(resolutionProp);
//        EditorGUILayout.PropertyField(autoUpdateProp);

//        EditorGUILayout.Space();

//        bool shapeSettingsChanged = false;
//        bool colorSettingsChanged = false;

//        EditorGUI.BeginChangeCheck();
//        EditorGUILayout.PropertyField(shapeSettingsProp, true);
//        if (EditorGUI.EndChangeCheck())
//        {
//            shapeSettingsChanged = true;
//        }

//        EditorGUI.BeginChangeCheck();
//        EditorGUILayout.PropertyField(colorSettingsProp, true);
//        if (EditorGUI.EndChangeCheck())
//        {
//            colorSettingsChanged = true;
//        }

//        EditorGUILayout.Space();

//        if (GUILayout.Button("Generate Planet"))
//        {
//            ((PlanetVisual)target).GeneratePlanetVisual();
//        }

//        serializedObject.ApplyModifiedProperties();

//        // Handle auto-update based on which settings changed
//        if (autoUpdateProp.boolValue)
//        {
//            if (shapeSettingsChanged)
//            {
//                ((PlanetVisual)target).OnShapeSettingsUpdated();
//            }

//            if (colorSettingsChanged)
//            {
//                ((PlanetVisual)target).OnColorSettingsUpdated();
//            }
//        }
//    }
//}