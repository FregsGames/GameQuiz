using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Questions
{
    public class QuestionGenerator : Singleton<QuestionGenerator>
    {
        public GamesContainer CurrentGamesContainer { get; set; }
        [SerializeField]
        private PlatformsContainer platformsDB;
        [SerializeField]
        private CompaniesContainer companiesDB;

        private void SetGamesContainer(GamesContainer gamesContainer)
        {
            CurrentGamesContainer = gamesContainer;
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
            //Vector2Int yearRange = DifficultyParameters.Instance.GetYearRange(difficulty);

            int year = CurrentGamesContainer.Years[Random.Range(0, CurrentGamesContainer.Years.Count - 1)];

            Game correctAnswer = CurrentGamesContainer.GetRandomGameFromYear(year, searchOnThatYear: true);

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(CurrentGamesContainer.GetRandomGameFromYear(year, searchOnThatYear: false).name);
            }

            return new Question("", $"Game from {year}", correctAnswer.name, otherOptions);
        }

        public Question GameNotFromYear(int options = 4, int difficulty = 1)
        {
            //Vector2Int yearRange = DifficultyParameters.Instance.GetYearRange(difficulty);

            int year = CurrentGamesContainer.Years[Random.Range(0, CurrentGamesContainer.Years.Count - 1)];

            List<Game> gamesNotFromYear = CurrentGamesContainer.GetXGamesFromYearX(options - 1, year);

            if (gamesNotFromYear.Count == 0)
                return null;

            Game correctAnswer = CurrentGamesContainer.GetRandomGameFromYear(year, searchOnThatYear: false);


            return new Question("", $"Game NOT from {year}", correctAnswer.name, gamesNotFromYear.Select(s => s.name).ToList());
        }

        public Question GameFromCompany(int options = 4, int difficulty = 1)
        {
            List<Company> companies = companiesDB.allCompanies.Where(
                c => c.developed.Length > DifficultyParameters.Instance.GetMinimumGameForCompany(difficulty)).ToList();

            Company randomCompnay = companies[Random.Range(0, companies.Count)];

            int involvedCompanyID = companiesDB.involved_companies.FirstOrDefault(ic => ic.company == randomCompnay.id).id;

            Game choosenGame = CurrentGamesContainer.GetFromCompany(involvedCompanyID, searchOnThatCompany: true);

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(CurrentGamesContainer.GetFromCompany(involvedCompanyID, searchOnThatCompany: false).name);
            }

            return new Question("", $"Game developed or published by {randomCompnay.name}", choosenGame.name, otherOptions);

        }

        public Question GameFromPlatform(int options = 4, int minimumGames = 1, int difficulty = 1)
        {
            Dictionary<int, Platform> validPlaforms = platformsDB.allPlatforms.Where(p => p.games.Count >= minimumGames).ToDictionary(t => t.id, t => t);

            int platformID = CurrentGamesContainer.Platforms[Random.Range(0, CurrentGamesContainer.Platforms.Count - 1)];

            Platform platform = platformsDB.allPlatforms.FirstOrDefault(p => p.id == platformID);

            Game[] potentialAnswers = CurrentGamesContainer.allGames.Where(g => g.platforms.Contains(platform.id)).ToArray();

            Game correctAnswer = potentialAnswers[Random.Range(0, potentialAnswers.Length)];

            List<string> otherOptions = new List<string>();

            for (int i = 0; i < options; i++)
            {
                otherOptions.Add(CurrentGamesContainer.GetFromPlatform(platform.id, searchOnThatPlatform: false).name); // could be the same
            }

            return new Question("", $"{platform.name} game", correctAnswer.name, otherOptions);
        }
    }
}
