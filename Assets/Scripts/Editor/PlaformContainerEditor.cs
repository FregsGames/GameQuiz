using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformsContainer))]
public class PlaformContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PlatformsContainer platformsContainer = (PlatformsContainer)target;

        foreach (var plat in platformsContainer.allPlatforms)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{plat.id}");
            GUILayout.Label(plat.name);
            GUILayout.EndHorizontal();
            if(EditorGUILayout.Foldout(false, "Games"))
            {
                foreach (var game in plat.games)
                {
                    GUILayout.Label(game.name);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
