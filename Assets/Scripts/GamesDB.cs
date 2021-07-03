using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamesDB : Singleton<GamesDB>
{
    public Dictionary<int, Game> allGames = new Dictionary<int, Game>();

    public void AddGame(Game game)
    {
        if (!allGames.ContainsKey(game.id))
        {
            allGames.Add(game.id, game);
        }
    }

    public Game GetFromCompany(int company, bool searchOnThatCompany)
    {
        List<Game> games = allGames.Values.ToList().Where(x => searchOnThatCompany ? x.involved_companies.Contains(company) 
        : !x.involved_companies.Contains(company)).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public Game GetFromPlatform(int platform, bool searchOnThatPlatform)
    {
        List<Game> games = allGames.Values.ToList().Where(x => searchOnThatPlatform ? x.platforms.Contains(platform)
        : !x.platforms.Contains(platform)).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public Game GetRandomGameFromYear(int year, bool searchOnThatYear)
    {
        List<Game> games = allGames.Values.ToList().Where(x => searchOnThatYear? x.realDate.Year == year : x.realDate.Year != year).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public List<Game> GetXGamesFromYearX(int count, int year)
    {
        List<Game> games = allGames.Values.ToList().Where(x => x.realDate.Year == year).ToList();

        var rnd = new System.Random();

        games = games.OrderBy(item => rnd.Next()).ToList();

        List<Game> result = new List<Game>();

        for (int i = 0; i < count; i++)
        {
            if(i > games.Count - 1)
            {
                break;
            }
            result.Add(games[i]);
        }

        return result;
    }
}
