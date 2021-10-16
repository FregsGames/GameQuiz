using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CupScriptable))]
public class CupScriptableEditor : Editor
{
    SerializedProperty id;
    SerializedProperty title;
    SerializedProperty packTitle;
    SerializedProperty levels;
    SerializedProperty cupImage;
    SerializedProperty state;
    SerializedProperty gamesContainer;
    SerializedProperty desc;
    SerializedProperty packDesc;

    List<Levels.Level> cupLevels;

    private void OnEnable()
    {
        //cupLevels = (serializedObject.targetObject as CupScriptable).levels;

        id = serializedObject.FindProperty("id");
        title = serializedObject.FindProperty("title");
        packTitle = serializedObject.FindProperty("packTitle");
        levels = serializedObject.FindProperty("levels");
        cupImage = serializedObject.FindProperty("cupImage");
        state = serializedObject.FindProperty("state");
        gamesContainer = serializedObject.FindProperty("gamesContainer");
        desc = serializedObject.FindProperty("desc");
        packDesc = serializedObject.FindProperty("packDesc");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(title);
        EditorGUILayout.PropertyField(packTitle);
        EditorGUILayout.PropertyField(cupImage);
        EditorGUILayout.PropertyField(state);
        EditorGUILayout.PropertyField(gamesContainer);
        EditorGUILayout.PropertyField(desc);
        EditorGUILayout.PropertyField(packDesc);
        EditorGUILayout.Separator(); 
        
        GUILayout.Label($"Levels:", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        for (int i = 0; i < levels.arraySize; i++)
        {
            EditorGUILayout.PropertyField(levels.GetArrayElementAtIndex(i));


            if (GUILayout.Button("Remove"))
            {
                ((CupScriptable)target).RemoveLevel(i);
                Repaint();
            }
        }

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (GUILayout.Button("Add Level"))
        {
            ((CupScriptable)target).AddLevel();
            Repaint();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
