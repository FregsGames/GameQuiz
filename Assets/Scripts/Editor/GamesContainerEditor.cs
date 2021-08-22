using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GamesContainer))]
public class GamesContainerEditor : Editor
{
    private GamesContainer gamesContainer;

    private const int MAX_PER_PAGE = 20;
    [SerializeField]
    private int page = 0;

    private bool editing = false;
    private int editingId;
    private int toDelete = -1;

    private void OnEnable()
    {
        gamesContainer = (GamesContainer)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if(toDelete != -1)
        {
            Game game = gamesContainer.allGames.FirstOrDefault(g => g.id == toDelete);
            gamesContainer.allGames.Remove(game);
            gamesContainer.OnGameDeleted?.Invoke(game.id);
            toDelete = -1;
        }

        ShowTopButtons();
        EditorGUILayout.Separator();

        for (int i = page * MAX_PER_PAGE; i < page * MAX_PER_PAGE + MAX_PER_PAGE; i++)
        {
            if (i > gamesContainer.allGames.Count - 1)
            {
                break;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label($"{gamesContainer.allGames[i].id}");

            if (editing && editingId == gamesContainer.allGames[i].id)
            {
                gamesContainer.allGames[i].name = GUILayout.TextField(gamesContainer.allGames[i].name);
            }
            else
            {
                GUILayout.Label(gamesContainer.allGames[i].name);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Date: ");
            GUILayout.Label($"{GamesContainer.GetDate(gamesContainer.allGames[i]):dd/MM/yyyy}");
            GUILayout.EndHorizontal();

            if (editing && editingId == gamesContainer.allGames[i].id)
            {
                if (GUILayout.Button("Remove"))
                {
                    toDelete = gamesContainer.allGames[i].id;
                    editing = false;
                }
                if (GUILayout.Button("Save"))
                {
                    editing = false;
                }
            }
            else if(!editing)
            {
                if (GUILayout.Button("Edit"))
                {
                    editingId = gamesContainer.allGames[i].id;
                    editing = true;
                }
            }

            EditorGUILayout.Separator();
        }

        if (GUILayout.Button("Remove Duplicates"))
        {
            page = 0;
            gamesContainer.RemoveDuplicates();
            Repaint();
        }

        if (GUILayout.Button("Clear"))
        {
            page = 0;
            gamesContainer.Clear();
            gamesContainer.OnAllGamesDeleted?.Invoke();
            Repaint();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowTopButtons()
    {
        GUILayout.Label($"{page}/{(gamesContainer.allGames.Count - 1) / MAX_PER_PAGE}", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 15 }, GUILayout.ExpandWidth(true));

        if (GUILayout.Button("To Start"))
        {
            page = 0;
            Repaint();
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("<"))
        {
            editing = false;
            page = page > 0 ? page - 1 : (gamesContainer.allGames.Count - 1) / MAX_PER_PAGE;
            Repaint();
        }
        if (GUILayout.Button(">"))
        {
            editing = false;
            page = page * MAX_PER_PAGE < gamesContainer.allGames.Count - 1 ? page + 1 : 0;
            Repaint();
        }

        GUILayout.EndHorizontal();
    }
}
