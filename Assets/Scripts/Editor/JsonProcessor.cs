using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class JsonProcessor : EditorWindow
{
    public enum JsonType { Platforms = 0, Games = 1, Companies = 2, InvolvedCompanies = 3 };

    PlatformsContainer platformsContainer;
    GamesContainer gamesContainer;

    TextAsset json;
    JsonType jsonType;

    [MenuItem("Window/Json Processor")]
    static void Init()
    {
        JsonProcessor window = (JsonProcessor)GetWindow(typeof(JsonProcessor));
        window.Show();
    }

    void OnGUI()
    {
        platformsContainer = (PlatformsContainer)EditorGUILayout.ObjectField(platformsContainer, typeof(PlatformsContainer), false);
        gamesContainer = (GamesContainer)EditorGUILayout.ObjectField(gamesContainer, typeof(GamesContainer), false);

        GUILayout.Label("JSON Processor", EditorStyles.boldLabel);
        GUILayout.Label("JsonFile");
        json = (TextAsset)EditorGUILayout.ObjectField(json, typeof(TextAsset), false);
        jsonType = (JsonType)EditorGUILayout.EnumPopup("Json type", jsonType);

        if (GUILayout.Button("Process"))
        {
            Process();
        }
    }

    private void Process()
    {
        switch (jsonType)
        {
            case JsonType.Platforms:
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(platformsContainer);
                ReadPlatforms();
                AssetDatabase.SaveAssets();
                break;
            case JsonType.Games:
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(gamesContainer);
                EditorUtility.SetDirty(platformsContainer);
                ReadGames();
                AssetDatabase.SaveAssets();
                break;
            case JsonType.Companies:
                break;
            case JsonType.InvolvedCompanies:
                // Same as platforms
                break;
            default:
                break;
        }
    }

    private void ReadGames()
    {
        Games games = JsonConvert.DeserializeObject<Games>(json.text);

        foreach (Game game in games.games)
        {
            gamesContainer.AddGame(game);
            platformsContainer.AddGame(game);
        }
    }

    private void ReadPlatforms()
    {
        Platforms platforms = JsonConvert.DeserializeObject<Platforms>(json.text);

        foreach (Platform platform in platforms.platforms)
        {
            platformsContainer.AddPlatform(platform);
        }
    }
}
