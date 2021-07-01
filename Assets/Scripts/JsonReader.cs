using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public GamesDB gamesDB;
    public PlatformsDB platformsDB;

    private void Start()
    {
        ReadPlatforms();
        ReadAllGames();
        ShowPlatforms();

    }

    public void ReadAllGames()
    {
        TextAsset[] jsons = Resources.LoadAll<TextAsset>("AllGames/");

        foreach (var json in jsons)
        {
            ReadGame(json);
        }
    }

    public void ReadPlatforms()
    {
        TextAsset[] jsons = Resources.LoadAll<TextAsset>("Platforms/");

        foreach (var json in jsons)
        {
            ReadPlatform(json);
        }

    }

    public void ReadPlatform(TextAsset jsonFile)
    {
        Platforms platforms = JsonConvert.DeserializeObject<Platforms>(jsonFile.text);

        foreach (Platform platform in platforms.platforms)
        {
            platformsDB.AddPlatform(platform);
        }
    }

    public void ReadGame(TextAsset jsonFile)
    {
        Games games = JsonConvert.DeserializeObject<Games>(jsonFile.text);

        foreach (Game game in games.games)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(game.first_release_date).ToLocalTime();
            game.realDate = dtDateTime;

            if (game.realDate.Year > 1985)
            {
                gamesDB.AddGame(game);
                platformsDB.AddGame(game);
            }
        }
    }

    public void ShowPlatforms()
    {
        foreach (var platform in platformsDB.allPlatforms.Values)
        {
            Debug.Log(platform.name);
            foreach (var game in platform.games)
            {
                Debug.Log(game.name);
            }
            Debug.Log("\n");
        }
    }

    public void ShowCompanies()
    {
        Dictionary<int, int> companies = new Dictionary<int, int>();

        foreach (var item in gamesDB.allGames.Values)
        {
            if (item.involved_companies == null)
                continue;


            if (!companies.ContainsKey(item.involved_companies[0]))
            {
                companies.Add(item.involved_companies[0], 1);
            }
        }

        Debug.Log("COMPANIES IDs");
        string comps = "";

        foreach (var company in companies.Keys)
        {
            comps += company + ",";
        }
        Debug.Log(comps);
        Debug.Log(companies.Count);
        Debug.Log(gamesDB.allGames.Count);
    }

    public void ShowYearStats()
    {
        Dictionary<int, int> years = new Dictionary<int, int>();

        foreach (var item in gamesDB.allGames.Values)
        {
            if (years.ContainsKey(item.realDate.Year))
            {
                years[item.realDate.Year]++;
            }
            else
            {
                years.Add(item.realDate.Year, 1);
            }
        }

        Debug.Log("YEARS");
        foreach (var year in years)
        {
            Debug.Log(year.Key + ": " + year.Value);
        }
    }
}
