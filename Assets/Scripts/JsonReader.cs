using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JsonReader : Singleton<JsonReader>
{
    private GamesDB gamesDB;
    private PlatformsDB platformsDB;
    private CompaniesDB companiesDB;

    private void Start()
    {
        gamesDB = GamesDB.Instance;
        platformsDB = PlatformsDB.Instance;
        companiesDB = CompaniesDB.Instance;

        ReadPlatforms();
        ReadAllGames();
        ReadInvolvedCompanies();
        ReadCompanies();
    }

    public void ExportAllGamesToJson()
    {
        Game[] games = gamesDB.allGames.Values.ToArray();
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Allgames_1.json", JsonConvert.SerializeObject(games));
    }

    public void ReadCompanies()
    {
        TextAsset[] jsons = Resources.LoadAll<TextAsset>("Companies/");

        foreach (var json in jsons)
        {
            ReadCompany(json);
        }
    }

    private void ReadCompany(TextAsset jsonFile)
    {
        Companies companies = JsonConvert.DeserializeObject<Companies>(jsonFile.text);

        foreach (Company company in companies.companies)
        {
            companiesDB.AddCompany(company);
        }
    }

    public void ReadInvolvedCompanies()
    {
        TextAsset[] jsons = Resources.LoadAll<TextAsset>("InvolvedCompanies/");

        foreach (var json in jsons)
        {
            ReadInvolvedCompany(json);
        }
    }

    private void ReadInvolvedCompany(TextAsset jsonFile)
    {
        Involved_Companies involved_Companies = JsonConvert.DeserializeObject<Involved_Companies>(jsonFile.text);

        foreach (Involved_Company involved_Company in involved_Companies.involved_Companies)
        {
            companiesDB.AddInvolvedCompany(involved_Company);
        }
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
                //companiesDB.AddGameToCompany(game);
            }
        }
    }

    public void ShowPlatforms()
    {
        foreach (var platform in platformsDB.allPlatforms.Values)
        {
            Debug.Log(platform.id + " " + platform.name);
            /*foreach (var game in platform.games)
            {
                Debug.Log(game.name);
            }*/
            //Debug.Log("\n");
        }
    }

    public void ShowCompanies()
    {
        Dictionary<int, Involved_Company> involved_companies = companiesDB.involved_companies;

        string comps = "";

        foreach (var item in involved_companies.Values)
        {
            comps += item.company + ",";
        }

        Debug.Log(comps);


        /*Dictionary<int, Company> involved_companies = companiesDB.involved_companies;

        Debug.Log("Companies count: " + involved_companies.Count);

        List<Company> filteredCompanies = involved_companies.Values.Where(c => c.games.Count > 2).ToList();

        Debug.Log("Companies with more than two games: " + filteredCompanies.Count);

        string comps = "";

        foreach (var item in filteredCompanies)
        {
            comps += item.id + ",";
        }

        Debug.Log(comps);*/
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
