using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformsContainer))]
public class PlaformContainerEditor : Editor
{
    PlatformsContainer platformsContainer;

    private bool editing;
    private Platform selectedPlatform;

    private bool showingGames;

    private void OnEnable()
    {
        platformsContainer = (PlatformsContainer)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (!showingGames)
        {
            ShowPlatforms();
        }
        else
        {
            ShowGames();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowGames()
    {
        if (GUILayout.Button("Back"))
        {
            selectedPlatform = null;
            showingGames = false;
            Repaint();
        }

        if (selectedPlatform == null)
            return;

        foreach (var game in selectedPlatform.games)
        {
            GUILayout.Label($"{game.name}");
        }
    }

    private void ShowPlatforms()
    {
        foreach (var plat in platformsContainer.allPlatforms)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{plat.id}");

            if (!editing || selectedPlatform.id != plat.id)
            {
                GUILayout.Label(plat.name);
            }
            else if (editing && selectedPlatform.id == plat.id)
            {
                plat.name = GUILayout.TextField(plat.name);
            }

            GUILayout.EndHorizontal();

            if (!editing)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Edit"))
                {
                    {
                        editing = true;
                        selectedPlatform = plat;
                        this.Repaint();
                    }
                }

                if (GUILayout.Button("Games"))
                {
                    {
                        showingGames = true;
                        selectedPlatform = plat;
                        this.Repaint();
                    }
                }
                GUILayout.EndHorizontal();
            }
            else if (editing && selectedPlatform.id == plat.id)
            {
                if (GUILayout.Button("Save"))
                {
                    {
                        editing = false;
                        selectedPlatform = null;
                        Repaint();
                    }
                }
            }
        }
    }
}
