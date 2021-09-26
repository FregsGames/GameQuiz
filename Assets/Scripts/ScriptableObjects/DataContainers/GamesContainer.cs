using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GamesContainer", menuName = "ScriptableObjects/Games")]
[InlineEditor]
public class GamesContainer : SerializedScriptableObject
{
    [SerializeField]
    public List<Game> allGames = new List<Game>();

    public Action<int> OnGameDeleted;
    public Action OnAllGamesDeleted;


    public static DateTime GetDate(Game game)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(game.first_release_date).ToLocalTime();
        return dtDateTime;
    }

    public Dictionary<int, int> Platforms()
    {
        Dictionary<int, int> platforms = new Dictionary<int, int>();
        foreach (var item in allGames)
        {
            foreach (var plat in item.platforms)
            {
                if (platforms.ContainsKey(plat))
                {
                    platforms[plat]++;
                }
                else
                {
                    platforms.Add(plat, 1);
                }
            }
        }
        return platforms;
    }

    public Dictionary<int, int> Involved()
    {
        Dictionary<int, int> companies = new Dictionary<int, int>();
        foreach (var item in allGames)
        {
            foreach (var plat in item.involved_companies)
            {
                if (companies.ContainsKey(plat))
                {
                    companies[plat]++;
                }
                else
                {
                    companies.Add(plat, 1);
                }
            }
        }
        return companies;
    }

    public Dictionary<int, int> Years()
    {
        Dictionary<int, int> years = new Dictionary<int, int>();
        foreach (var item in allGames)
        {
            int year = GetDate(item).Year;
            if (years.ContainsKey(year))
            {
                years[year]++;
            }
            else
            {
                years.Add(year, 1);
            }
        }
        return years;
    }

    public void AddGame(Game game)
    {
        if (!allGames.Contains(game))
        {
            allGames.Add(game);
        }
    }

    public void Clear()
    {
        allGames.Clear();
        OnAllGamesDeleted?.Invoke();
    }

    public Game GetFromCompany(List<Involved_Company> involved_Companies, bool searchOnThatCompany)
    {
        /*List<Game> games = allGames.Where(x => searchOnThatCompany ? x.involved_companies.Any(i => x.involved_companies.Contains(i))
        : !x.involved_companies.Any(i => x.involved_companies.Contains(i))).ToList();*/

        List<int> involvedIds = involved_Companies.Select(i => i.id).ToList();

        List<Game> games = new List<Game>();

        if (searchOnThatCompany)
        {
            games = allGames.Where(x => x.involved_companies.Any(i => involvedIds.Contains(i))).ToList();
        }
        else
        {
            games = allGames.Where(x => !x.involved_companies.Any(i => involvedIds.Contains(i))).ToList();
        }

        return games[Random.Range(0, games.Count)];
    }

    public Game GetFromPlatform(int platform, bool searchOnThatPlatform)
    {
        List<Game> games = allGames.Where(x => searchOnThatPlatform ? x.platforms.Contains(platform)
        : !x.platforms.Contains(platform)).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public Game GetRandomGameFromYear(int year, bool searchOnThatYear)
    {
        List<Game> games = allGames.Where(x => searchOnThatYear ? GetDate(x).Year == year: GetDate(x).Year != year).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public Game GetRandomGameFromYear(int year, bool searchOnThatYear, int[] toExclude)
    {
        List<Game> games = allGames.Where(x => searchOnThatYear ? GetDate(x).Year == year && !toExclude.Contains(x.id): GetDate(x).Year != year && !toExclude.Contains(x.id)).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public Game GetFromPlatform(int platform, bool searchOnThatPlatform, int[] toExclude)
    {
        List<Game> games = allGames.Where(x => searchOnThatPlatform ? x.platforms.Contains(platform) && !toExclude.Contains(x.id)
        : !x.platforms.Contains(platform) && !toExclude.Contains(x.id)).ToList();

        return games[Random.Range(0, games.Count)];
    }
    public Game GetFromCompany(int company, bool searchOnThatCompany, int[] toExclude)
    {
        List<Game> games = allGames.Where(x => searchOnThatCompany ? x.involved_companies.Contains(company) && !toExclude.Contains(x.id)
        : !x.involved_companies.Contains(company) && !toExclude.Contains(x.id)).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public List<Game> GetXGamesFromYearX(int count, int year)
    {
        List<Game> games = allGames.Where(x => GetDate(x).Year == year).ToList();

        if (games.Count < count)
            return new List<Game>();

        var rnd = new System.Random();

        games = games.OrderBy(item => rnd.Next()).ToList();

        List<Game> result = new List<Game>();

        for (int i = 0; i < count; i++)
        {
            if (i > games.Count - 1)
            {
                break;
            }
            result.Add(games[i]);
        }

        return result;
    }

    public void RemoveDuplicates()
    {
        allGames = allGames.GroupBy(x => x.id).Select(y => y.First()).ToList();
    }
}
