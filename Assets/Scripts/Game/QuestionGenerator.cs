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

        public Question FromTemplate(QuestionTemplate template, LevelScriptable level)
        {
            Question question = null;

            switch (template.ContentType)
            {
                case QuestionTemplate.QuestionContent.fromYear:
                    question = GameFromYear(level.years);
                    break;
                case QuestionTemplate.QuestionContent.fromPlatform:
                    question = GameFromPlatform(level.platforms.Select(p => p.platform).ToList());
                    question = GetRandomGenericQuestion(level);
                    break;
                case QuestionTemplate.QuestionContent.fromCompany:

                    IEnumerable<CompanyTuple> enumerable = level.companies.Where(c => c.counter >= 3);

                    IEnumerable<int> selection = enumerable.Select(p => p.company.id);

                    question = GameFromCompany(selection.ToList());
                    break;
                case QuestionTemplate.QuestionContent.notFromYear:
                    question = GameNotFromYear();
                    break;
                case QuestionTemplate.QuestionContent.notFromPlatform:
                    question = GameNotFromPlatform();
                    break;
            }
            return question;
        }

        public Question GetRandomGenericQuestion(LevelScriptable level)
        {
            var random = Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    return GameFromYear(level.years);
                case 1:
                    return GameFromPlatform(level.platforms.Select(p => p.platform).ToList());
                default:
                    return GameFromCompany(level.companies.Where(c => c.counter >= 3).Select(p => p.company.id).ToList());
            }
        }

        public Question GameFromYear(List<int> years, int options = 4)
        {
            List<int> validYears = new List<int>();

            if (years != null)
            {
                validYears = years;
            }
            else
            {
                validYears = CurrentGamesContainer.Years().Keys.ToList();
            }

            int year = validYears[Random.Range(0, validYears.Count)];

            Game correctAnswer = CurrentGamesContainer.GetRandomGameFromYear(year, searchOnThatYear: true);
            List<Game> otherOptions = new List<Game>();

            for (int i = 1; i < options; i++)
            {
                Game other = CurrentGamesContainer.GetRandomGameFromYear(year, searchOnThatYear: false, otherOptions.Select(g => g.id).ToArray());
                otherOptions.Add(other);
            }
            string statement = Translations.instance.GetText("s_year_0");

            return new Question("", $"{statement} {year}", correctAnswer.name, otherOptions.Select(g => g.name).ToList());
        }

        public Question GameFromPlatform(List<int> platforms, int options = 4)
        {
            List<int> validPlats = new List<int>();

            if (platforms != null)
            {
                validPlats = platforms;
            }
            else
            {
                validPlats = CurrentGamesContainer.Platforms().Keys.ToList();
            }

            bool platformObtained = false;
            int platform = 0;
            int tries = 0;
            Game correctAnswer = new Game();
            List<Game> otherOptions = new List<Game>();

            while (!platformObtained && tries < 20)
            {
                platform = validPlats[Random.Range(0, validPlats.Count)];

                correctAnswer = CurrentGamesContainer.GetFromPlatform(platform, true);
                platformObtained = true;

                for (int i = 1; i < options; i++)
                {
                    Game other = CurrentGamesContainer.GetFromPlatform(platform, searchOnThatPlatform: false, otherOptions.Select(g => g.id).ToArray());
                    if (other == null)
                    {
                        otherOptions.Clear();
                        platformObtained = false;
                        tries++;
                        break;
                    }
                    else
                    {
                        otherOptions.Add(other);
                    }

                }
            }

            if (otherOptions.Count == 0)
            {
                return null;
            }

            string statement = Translations.instance.GetText($"s_plat_{Random.Range(0, 2)}");

            return new Question("", $"{statement} {platformsDB.GetName(platform)}", correctAnswer.name, otherOptions.Select(g => g.name).ToList());
        }

        public Question GameFromCompany(List<int> companies, int options = 4, int methodTries = 0)
        {
            List<int> validCompanies = new List<int>();

            if (companies != null)
            {
                validCompanies = companies;
            }
            else
            {
                validCompanies = CurrentGamesContainer.Involved().Keys.ToList();
            }

            bool validCompanyGot = false;
            int tries = 0;

            int company = 0;
            List<Involved_Company> involved = new List<Involved_Company>();

            while (!validCompanyGot && tries < 20)
            {
                company = validCompanies[Random.Range(0, validCompanies.Count)];
                involved = companiesDB.involved_companies.Where(c => c.company == company && c.developer).ToList();
                validCompanyGot = involved.Count > 0;

                tries++;
            }

            if (!validCompanyGot)
            {
                if (methodTries < 10)
                {
                    int mTries = methodTries + 1;
                    return GameFromCompany(companies, methodTries: mTries);
                }
                else
                {
                    return null;
                }
            }

            Game correctAnswer = CurrentGamesContainer.GetFromCompany(involved, true);
            List<Game> otherOptions = new List<Game>();


            for (int i = 1; i < options; i++)
            {
                Game other = CurrentGamesContainer.GetFromCompany(involved, false);
                otherOptions.Add(other);
            }

            string statement = Translations.instance.GetText("s_developedBy_0");

            return new Question("", $"{statement} {companiesDB.GetName(company)}", correctAnswer.name, otherOptions.Select(g => g.name).ToList());
        }

        public Question GameNotFromYear(int options = 4, int difficulty = 1)
        {
            List<int> years = CurrentGamesContainer.Years().Where(y => y.Value >= options).Select(yr => yr.Key).ToList();

            int year = years[Random.Range(0, years.Count)];

            List<Game> gamesFromYear = CurrentGamesContainer.GetXGamesFromYearX(options - 1, year);

            if (gamesFromYear.Count == 0)
                return null;

            Game correctAnswer = CurrentGamesContainer.GetRandomGameFromYear(year, searchOnThatYear: false);


            return new Question("", $"Game NOT from {year}", correctAnswer.name, gamesFromYear.Select(s => s.name).ToList());
        }

        public Question GameFromPlatform(int options = 4, int minimumGames = 1, int difficulty = 1)
        {
            List<int> allPlatforms = CurrentGamesContainer.Platforms().Where(c => c.Value >= minimumGames).Select(yr => yr.Key).ToList();
            int platformID = allPlatforms[Random.Range(0, allPlatforms.Count)];

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

        public Question GameNotFromPlatform(int options = 4, int minimumGames = 1, int difficulty = 1)
        {
            Game correctAnswer = null;
            List<Game> gamesForPlatform = new List<Game>();
            int platform = -1;
            int tries = 0;

            while (correctAnswer == null && tries < 20)
            {
                List<int> platforms = CurrentGamesContainer.Platforms().Where(p => p.Value >= options).Select(plt => plt.Key).ToList();
                platform = platforms[Random.Range(0, platforms.Count)];

               gamesForPlatform = CurrentGamesContainer.GetXGamesFromPlafformX(options - 1, platform);

                if (gamesForPlatform.Count == 0)
                    return null;

                correctAnswer = CurrentGamesContainer.GetRandomGameNotForPlaform(platform);

            }

            string platName = platformsDB.allPlatforms.FirstOrDefault(p => p.id == platform).name;
            return new Question("", $"Game NOT playeable in {platName}", correctAnswer.name, gamesForPlatform.Select(s => s.name).ToList());
        }
    }
}
