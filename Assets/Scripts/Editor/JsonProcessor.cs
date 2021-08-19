using Newtonsoft.Json;
using System;
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
            EditorUtility.SetDirty(this);
            Process();
        }
    }

    private void Process()
    {
        gamesContainer.OnGameDeleted = platformsContainer.OnGameDeleted;
        gamesContainer.OnAllGamesDeleted = platformsContainer.ClearAllGames;

        switch (jsonType)
        {
            case JsonType.Platforms:
                Undo.RecordObject(platformsContainer, "Platform container read");
                ReadPlatforms();
                EditorUtility.SetDirty(platformsContainer);
                break;
            case JsonType.Games:
                Undo.RecordObject(gamesContainer, "Games container read");
                Undo.RecordObject(platformsContainer, "Platform container read");
                ReadGames();
                EditorUtility.SetDirty(platformsContainer);
                EditorUtility.SetDirty(gamesContainer);
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
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(game.first_release_date).ToLocalTime();
            game.realDate = dtDateTime;

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
