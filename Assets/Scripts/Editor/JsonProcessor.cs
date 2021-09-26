using Newtonsoft.Json;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonProcessor : EditorWindow
{
    public enum JsonType { Platforms = 0, Games = 1, Companies = 2, InvolvedCompanies = 3 };

    PlatformsContainer platformsContainer;
    GamesContainer gamesContainer;
    CompaniesContainer companiesContainer;

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
        companiesContainer = (CompaniesContainer)EditorGUILayout.ObjectField(companiesContainer, typeof(CompaniesContainer), false);

        GUILayout.Label("JSON Processor", EditorStyles.boldLabel);
        GUILayout.Label("JsonFile");
        json = (TextAsset)EditorGUILayout.ObjectField(json, typeof(TextAsset), false);
        jsonType = (JsonType)EditorGUILayout.EnumPopup("Json type", jsonType);

        if (GUILayout.Button("Process"))
        {
            Process();
        }

        if (jsonType == JsonType.Games)
        {
            WriteInvolvedCompaniesRequest();
        }

        if (jsonType == JsonType.InvolvedCompanies)
        {
            WriteCompaniesRequest();
        }
    }

    private void WriteCompaniesRequest()
    {
        if (GUILayout.Button("Generate Companies Request"))
        {
            string companiesRequest = "fields name, developed; where id = (";

            Involved_Companies involved_companies = JsonConvert.DeserializeObject<Involved_Companies>(json.text);

            foreach (Involved_Company inv in involved_companies.involved_Companies)
            {
                companiesRequest += $"{inv.company},";
            }

            companiesRequest = companiesRequest.Substring(0, companiesRequest.Length - 1); // remove last comma
            companiesRequest += "); limit 500;";

            File.WriteAllText(Application.persistentDataPath + "/requestCompaniesTemp.txt", companiesRequest);
        }
    }

    private void WriteInvolvedCompaniesRequest()
    {
        if (GUILayout.Button("Generate Involved Request"))
        {
            string involvedCompaniesRequest = "fields company, developer; where id = (";

            Games games = JsonConvert.DeserializeObject<Games>(json.text);

            foreach (Game game in games.games)
            {
                foreach (var item in game.involved_companies)
                {
                    involvedCompaniesRequest += $"{item},";
                }
            }

            involvedCompaniesRequest = involvedCompaniesRequest.Substring(0, involvedCompaniesRequest.Length - 1); // remove last comma
            involvedCompaniesRequest += "); limit 500;";

            File.WriteAllText(Application.persistentDataPath + "/requestInvolvedTemp.txt", involvedCompaniesRequest);
        }
    }

    private void Process()
    {
        gamesContainer.OnGameDeleted = platformsContainer.OnGameDeleted;
        gamesContainer.OnAllGamesDeleted = platformsContainer.ClearAllGames;

        switch (jsonType)
        {
            case JsonType.Platforms:
                ReadPlatforms();
                break;
            case JsonType.Games:
                ReadGames();
                break;
            case JsonType.Companies:
                ReadCompanies();
                break;
            case JsonType.InvolvedCompanies:
                ReadInvolvedCompanies();
                break;
            default:
                break;
        }
    }

    private void ReadCompanies()
    {
        string rawJson = json.text;

        if (json.text[0] != '{')
        {
            rawJson = string.Concat("{\"Companies\":", rawJson, "}");
        }

        Companies companies = JsonConvert.DeserializeObject<Companies>(rawJson);

        foreach (Company company in companies.companies)
        {
            companiesContainer.AddCompany(company);
        }
    }

    private void ReadInvolvedCompanies()
    {
        string rawJson = json.text;

        if (json.text[0] != '{')
        {
            rawJson = string.Concat("{\"Involved_Companies\":", rawJson, "}");
        }

        Involved_Companies companies = JsonConvert.DeserializeObject<Involved_Companies>(rawJson);

        foreach (Involved_Company company in companies.involved_Companies)
        {
            companiesContainer.AddInvolvedCompany(company);
        }
    }

    private void ReadGames()
    {
        string rawJson = json.text;

        if (json.text[0] != '{')
        {
            rawJson = string.Concat("{\"Games\":", rawJson, "}");
        }

        Games games = JsonConvert.DeserializeObject<Games>(rawJson);

        foreach (Game game in games.games)
        {
            gamesContainer.AddGame(game);
            platformsContainer.AddGame(game);
        }
    }

    private void ReadPlatforms()
    {
        string rawJson = json.text;

        if (json.text[0] != '{')
        {
            rawJson = string.Concat("{\"Platforms\":", rawJson, "}");
        }

        Platforms platforms = JsonConvert.DeserializeObject<Platforms>(rawJson);

        foreach (Platform platform in platforms.platforms)
        {
            platformsContainer.AddPlatform(platform);
        }
    }
}
