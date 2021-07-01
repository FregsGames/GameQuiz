using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Questions
{
    public class QuestionGenerator
    {
        [SerializeField]
        Vector2Int yearRange = new Vector2Int(1986, 2020);

        public Question GameFromYear(int options)
        {
            GamesDB gamesDB = GameObject.FindObjectOfType<GamesDB>();

            int year = UnityEngine.Random.Range(yearRange.x, yearRange.y);

            Game correctAnswer = gamesDB.GetRandomGameFromYear(year, searchOnThatYear: true);

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetRandomGameFromYear(year, searchOnThatYear: false).name);
            }

            return new Question($"Game from {year}", correctAnswer.name, otherOptions);
        }

        public Question GameFromCompany(int options)
        {
            GamesDB gamesDB = GameObject.FindObjectOfType<GamesDB>();

            List<Game> allGames = gamesDB.allGames.Values.ToList();

            Game correctAnswer = allGames[UnityEngine.Random.Range(0, allGames.Count)];

            int company = correctAnswer.involved_companies[0];

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetFromCompany(company, searchOnThatCompany: false).name);
            }

            return new Question($"Game developed by {company}", correctAnswer.name, otherOptions);
        }

        public Question GameFromPlatform(int options, int minimumGames = 1)
        {
            GamesDB gamesDB = GameObject.FindObjectOfType<GamesDB>();
            PlatformsDB platformsDB = GameObject.FindObjectOfType<PlatformsDB>();

            Dictionary<int, Platform> validPlaforms = platformsDB.allPlatforms.Where(p => p.Value.games.Count >= minimumGames).ToDictionary(t => t.Key, t => t.Value);

            List<Platform> validPlatformsList = validPlaforms.Values.ToList();

            Platform platform = validPlatformsList[UnityEngine.Random.Range(0, validPlatformsList.Count)];

            Game correctAnswer = gamesDB.allGames.Values.First(g => g.platforms.Contains(platform.id));

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetFromPlatform(platform.id, searchOnThatPlatform: false).name);
            }

            return new Question($"{platform.name} game", correctAnswer.name, otherOptions);
        }
    }
}
