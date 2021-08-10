using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Questions
{
    public class QuestionGenerator : Singleton<QuestionGenerator>
    {
        private GamesDB gamesDB;
        private PlatformsDB platformsDB;
        private CompaniesDB companiesDB;

        private void Start()
        {
            if (gamesDB == null)
            {
                gamesDB = GamesDB.Instance;
            }

            if (platformsDB == null)
            {
                platformsDB = PlatformsDB.Instance;
            }

            if (companiesDB == null)
            {
                companiesDB = CompaniesDB.Instance;
            }
        }

        public Question FromTemplate(QuestionTemplate template)
        {
            switch (template.ContentType)
            {
                case QuestionTemplate.QuestionContent.fromYear:
                    return GameFromYear(difficulty: int.Parse(template.ExtraData));
                case QuestionTemplate.QuestionContent.fromPlatform:
                    return GameFromPlatform(difficulty: int.Parse(template.ExtraData));
                case QuestionTemplate.QuestionContent.fromCompany:
                    return GameFromCompany(difficulty: int.Parse(template.ExtraData));
                case QuestionTemplate.QuestionContent.handwriten:
                    return GetRandomGenericQuestion(4);
                case QuestionTemplate.QuestionContent.notFromYear:
                    return GameNotFromYear(difficulty: int.Parse(template.ExtraData));
                case QuestionTemplate.QuestionContent.notFromPlatform:
                    return GetRandomGenericQuestion(4);
                case QuestionTemplate.QuestionContent.notFromCompany:
                    return GetRandomGenericQuestion(4);
                default:
                    return GetRandomGenericQuestion(4);
            }
        }

        public Question GetRandomGenericQuestion(int options)
        {
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

        public Question GameFromYear(int options = 4, int difficulty = 1)
        {
            Vector2Int yearRange = DifficultyParameters.Instance.GetYearRange(difficulty);

            int year = Random.Range(yearRange.x, yearRange.y);

            Game correctAnswer = gamesDB.GetRandomGameFromYear(year, searchOnThatYear: true);

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetRandomGameFromYear(year, searchOnThatYear: false).name);
            }

            return new Question("", $"Game from {year}", correctAnswer.name, otherOptions);
        }

        public Question GameNotFromYear(int options = 4, int difficulty = 1)
        {
            Vector2Int yearRange = DifficultyParameters.Instance.GetYearRange(difficulty);

            int year = Random.Range(yearRange.x, yearRange.y);

            List<Game> gamesNotFromYear = gamesDB.GetXGamesFromYearX(options - 1, year);

            if (gamesNotFromYear.Count == 0)
                return null;

            Game correctAnswer = gamesDB.GetRandomGameFromYear(year, searchOnThatYear: false);


            return new Question("", $"Game NOT from {year}", correctAnswer.name, gamesNotFromYear.Select(s => s.name).ToList());
        }

        public Question GameFromCompany(int options = 4, int difficulty = 1)
        {
            List<Company> companies = companiesDB.allCompanies.Values.Where(
                c => c.developed.Length > DifficultyParameters.Instance.GetMinimumGameForCompany(difficulty)).ToList();

            Company randomCompnay = companies[Random.Range(0, companies.Count)];

            int involvedCompanyID = companiesDB.involved_companies.FirstOrDefault(ic => ic.Value.company == randomCompnay.id).Key;

            Game choosenGame = gamesDB.GetFromCompany(involvedCompanyID, searchOnThatCompany: true);

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(gamesDB.GetFromCompany(involvedCompanyID, searchOnThatCompany: false).name);
            }

            return new Question("", $"Game developed or published by {randomCompnay.name}", choosenGame.name, otherOptions);

        }

        public Question GameFromPlatform(int options = 4, int minimumGames = 1, int difficulty = 1)
        {
            Dictionary<int, Platform> validPlaforms = platformsDB.allPlatforms.Where(p => p.Value.games.Count >= minimumGames).ToDictionary(t => t.Key, t => t.Value);

            if(difficulty < 3)
            {
                int[] platformFilter = DifficultyParameters.Instance.GetPlatforms(difficulty);
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

            return new Question("", $"{platform.name} game", correctAnswer.name, otherOptions);
        }
    }
}
