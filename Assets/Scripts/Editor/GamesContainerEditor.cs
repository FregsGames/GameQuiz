using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GamesContainer))]
public class GamesContainerEditor : Editor
{
    GamesContainer gamesContainer;
    private void OnEnable()
    {
        gamesContainer = (GamesContainer)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        foreach (var game in gamesContainer.allGames)
        {
            GUILayout.Label(game.name);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
