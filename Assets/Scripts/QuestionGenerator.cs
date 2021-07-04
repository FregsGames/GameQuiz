using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Questions
{
    public class QuestionGenerator
    {
        private GamesDB gamesDB;
        private PlatformsDB platformsDB;
        private CompaniesDB companiesDB;

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

            if (companiesDB == null)
            {
                companiesDB = CompaniesDB.instance;
            }

            var random = Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    return GameFromCompany(options);
                case 1:
                    return GameFromPlatform(options);
                default:
                    return GameFromYear(options);
            }
        }

        public Question GameFromYear(int options, int difficulty = 1)
        {
            Vector2Int yearRange = DifficultyParameters.instance.GetYearRange(difficulty);

            int year = Random.Range(yearRange.x, yearRange.y);

            Game correctAnswer = gamesDB.GetRandomGameFromYear(year, searchOnThatYear: true);

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetRandomGameFromYear(year, searchOnThatYear: false).name);
            }

            return new Question($"Game from {year}", correctAnswer.name, otherOptions);
        }

        public Question GameFromCompany(int options, int difficulty = 1)
        {
            List<Company> companies = companiesDB.allCompanies.Values.Where(
                c => c.developed.Length > DifficultyParameters.instance.GetMinimumGameForCompany(difficulty)).ToList();

            Company randomCompnay = companies[Random.Range(0, companies.Count)];

            int involvedCompanyID = companiesDB.involved_companies.FirstOrDefault(ic => ic.Value.company == randomCompnay.id).Key;

            Game choosenGame = gamesDB.GetFromCompany(involvedCompanyID, searchOnThatCompany: true);

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetFromCompany(involvedCompanyID, searchOnThatCompany: false).name);
            }

            return new Question($"Game developed or published by {randomCompnay.name}", choosenGame.name, otherOptions);

        }

        public Question GameFromPlatform(int options, int minimumGames = 1, int difficulty = 1)
        {
            Dictionary<int, Platform> validPlaforms = platformsDB.allPlatforms.Where(p => p.Value.games.Count >= minimumGames).ToDictionary(t => t.Key, t => t.Value);

            if(difficulty < 3)
            {
                int[] platformFilter = DifficultyParameters.instance.GetPlatforms(difficulty);
                validPlaforms = platformsDB.allPlatforms.Where(p => platformFilter.Contains(p.Key)).ToDictionary(t => t.Key, t => t.Value);
            }


            List<Platform> validPlatformsList = validPlaforms.Values.ToList();

            Platform platform = validPlatformsList[Random.Range(0, validPlatformsList.Count)];

            Game[] potentialAnswers = gamesDB.allGames.Values.Where(g => g.platforms.Contains(platform.id)).ToArray();

            Game correctAnswer = potentialAnswers[Random.Range(0, potentialAnswers.Length)];

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetFromPlatform(platform.id, searchOnThatPlatform: false).name); // could be the same
            }

            return new Question($"{platform.name} game", correctAnswer.name, otherOptions);
        }
    }
}
