using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Questions
{
    public class QuestionGenerator
    {
        [SerializeField]
        Vector2Int yearRange = new Vector2Int(1986, 2020);

        private GamesDB gamesDB;
        private PlatformsDB platformsDB;

        public Question GetRandomGenericQuestion(int options)
        {
            if(gamesDB == null)
            {
                gamesDB = GamesDB.instance;
            }

            if(platformsDB == null)
            {
                platformsDB = PlatformsDB.instance;
            }


            var random = Random.Range(0, 2);

            switch (random)
            {
                case 0:
                    return GameFromYear(options);
                case 1:
                    return GameFromPlatform(options);
                default:
                    return GameFromCompany(options);
            }
        }

        public Question GameFromYear(int options, int difficulty = 0)
        {
            int year = Random.Range(yearRange.x, yearRange.y);

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
            List<Game> allGames = gamesDB.allGames.Values.ToList();

            Game correctAnswer = allGames[Random.Range(0, allGames.Count)];

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
            Dictionary<int, Platform> validPlaforms = platformsDB.allPlatforms.Where(p => p.Value.games.Count >= minimumGames).ToDictionary(t => t.Key, t => t.Value);

            List<Platform> validPlatformsList = validPlaforms.Values.ToList();

            Platform platform = validPlatformsList[Random.Range(0, validPlatformsList.Count)];

            Game[] potentialAnswers = gamesDB.allGames.Values.Where(g => g.platforms.Contains(platform.id)).ToArray();

            Game correctAnswer = potentialAnswers[Random.Range(0, potentialAnswers.Length)];

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetFromPlatform(platform.id, searchOnThatPlatform: false).name);
            }

            return new Question($"{platform.name} game", correctAnswer.name, otherOptions);
        }
    }
}
