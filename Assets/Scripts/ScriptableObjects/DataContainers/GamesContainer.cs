using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "GamesContainer", menuName = "ScriptableObjects/Games")]
public class GamesContainer : ScriptableObject
{
    [SerializeField]
    public List<Game> allGames = new List<Game>();


    public Action<int> OnGameDeleted;
    public Action OnAllGamesDeleted;

    public void AddGame(Game game)
    {
        Debug.Log(game.name);
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

    public Game GetFromCompany(int company, bool searchOnThatCompany)
    {
        List<Game> games = allGames.Where(x => searchOnThatCompany ? x.involved_companies.Contains(company)
        : !x.involved_companies.Contains(company)).ToList();

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
        List<Game> games = allGames.Where(x => searchOnThatYear ? x.realDate.Year == year : x.realDate.Year != year).ToList();

        return games[Random.Range(0, games.Count)];
    }

    public List<Game> GetXGamesFromYearX(int count, int year)
    {
        List<Game> games = allGames.Where(x => x.realDate.Year == year).ToList();

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
