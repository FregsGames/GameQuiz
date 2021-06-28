using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamesDB : MonoBehaviour
{
    public Dictionary<int, Game> allGames = new Dictionary<int, Game>();

    public void AddGame(Game game)
    {
        if (!allGames.ContainsKey(game.id))
        {
            allGames.Add(game.id, game);
        }
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
