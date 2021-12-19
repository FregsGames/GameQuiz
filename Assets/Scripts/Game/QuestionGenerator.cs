using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Questions
{
    public class QuestionGenerator : Singleton<QuestionGenerator>
    {
        [SerializeField]
        private GamesC gamesDB;

        System.Random rnd = new System.Random();

        public Question FromTemplate(QuestionTemplate template, List<GameFilter> filters, List<string> answersToExlude)
        {
            Question question = null;

            switch (template.ContentType)
            {
                case QuestionTemplate.QuestionContent.fromYear:
                    question = GameFromYear(filters, answersToExlude: answersToExlude);
                    break;
                case QuestionTemplate.QuestionContent.fromPlatform:
                    question = GameFromPlatform(filters, answersToExlude: answersToExlude);
                    break;
                case QuestionTemplate.QuestionContent.fromCompany:
                    question = GameFromCompany(filters, answersToExlude: answersToExlude);
                    break;
                case QuestionTemplate.QuestionContent.notFromYear:
                    question = GameFromYear(filters, true, answersToExlude: answersToExlude);
                    break;
                case QuestionTemplate.QuestionContent.notFromPlatform:
                    question = GameFromPlatform(filters, true, answersToExlude: answersToExlude);
                    break;
                case QuestionTemplate.QuestionContent.notFromCompany:
                    question = GameFromCompany(filters, true, answersToExlude: answersToExlude);
                    break;
                case QuestionTemplate.QuestionContent.protagonists:
                    question = ProtagonistFromGame(filters, answersToExlude: answersToExlude);
                    break;
                case QuestionTemplate.QuestionContent.notProtagonist:
                    question = ProtagonistFromGame(filters, true, answersToExlude: answersToExlude);
                    break;
            }
            return question;
        }

        public Question GetRandomGenericQuestion(List<GameFilter> filters)
        {
            var random = Random.Range(0, 6);

            switch (random)
            {
                case 0:
                    return GameFromYear(filters);
                case 1:
                    return GameFromPlatform(filters);
                case 2:
                    return GameFromCompany(filters, true);
                case 3:
                    return GameFromYear(filters, true);
                case 4:
                    return GameFromPlatform(filters, true);
                default:
                    return GameFromCompany(filters);
            }
        }

        public Question GameFromYear(List<GameFilter> filters, bool inverseQuestion = false, List<string> answersToExlude = null)
        {
            List<GameC> filteredGamesForAnswer, filteredGamesForOtherOptions;
            GetFilteredListsOfGames(filters, out filteredGamesForAnswer, out filteredGamesForOtherOptions);

            Dictionary<int, int> years = GetYearsFromGames(filteredGamesForAnswer);

            int year = 0;
            string correctAnswer = "";
            List<string> otherOptions = new List<string>();
            string statement = "";

            if (inverseQuestion)
            {
                List<int> validYears = years.Where(y => y.Value >= 3).Select(x => x.Key).ToList();
                if (validYears == null || validYears.Count == 0)
                {
                    Debug.Log("Not enough games to do an inverse question.");
                    year = years.Keys.ToList()[Random.Range(0, years.Keys.Count)];

                    rnd = new System.Random();

                    if (answersToExlude != null)
                    {
                        if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                        {
                            Debug.LogError("Cannot exclude answer on game from year, getting duplicated question");
                        }
                        else
                        {
                            filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                        }
                    }

                    correctAnswer = filteredGamesForAnswer.Where(g => g.year == year).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                    otherOptions = filteredGamesForOtherOptions.Where(g => g.year != year).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();
                    statement = Translations.instance.GetText("s_year_0");
                }
                else
                {
                    rnd = new System.Random();
                    year = validYears[Random.Range(0, validYears.Count)];

                    if (answersToExlude != null)
                    {
                        if (filteredGamesForOtherOptions.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                        {
                            Debug.LogError("Cannot exclude answer on game from year inverse, getting duplicated question");
                        }
                        else
                        {
                            filteredGamesForOtherOptions = filteredGamesForOtherOptions.Where(g => !answersToExlude.Contains(g.name)).ToList();
                        }
                    }

                    correctAnswer = filteredGamesForOtherOptions.Where(g => g.year != year).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                    otherOptions = filteredGamesForAnswer.Where(g => g.year == year).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();
                    statement = Translations.instance.GetText("s_year_not");
                }
            }
            else
            {
                year = years.Keys.ToList()[Random.Range(0, years.Keys.Count)];

                rnd = new System.Random();

                if (answersToExlude != null)
                {
                    if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                    {
                        Debug.LogError("Cannot exclude answer on game from year, getting duplicated question");
                    }
                    else
                    {
                        filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                    }
                }

                correctAnswer = filteredGamesForAnswer.Where(g => g.year == year).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                otherOptions = filteredGamesForOtherOptions.Where(g => g.year != year).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();
                statement = Translations.instance.GetText("s_year_0");
            }

            return new Question("", $"{statement} {year}", correctAnswer, otherOptions);
        }

        private void GetFilteredListsOfGames(List<GameFilter> filters, out List<GameC> filteredGamesForAnswer, out List<GameC> filteredGamesForOtherOptions)
        {
            filteredGamesForAnswer = new List<GameC>(gamesDB.gamesC);
            filteredGamesForOtherOptions = new List<GameC>(gamesDB.gamesC);

            foreach (var filter in filters)
            {
                filteredGamesForAnswer = filter.Filter(filteredGamesForAnswer);

                if (filter.otherOptionsUseFilter)
                {
                    filteredGamesForOtherOptions = filter.Filter(filteredGamesForOtherOptions);
                }
            }
        }

        private static Dictionary<int, int> GetYearsFromGames(List<GameC> filteredGames)
        {
            Dictionary<int, int> years = new Dictionary<int, int>();

            foreach (var game in filteredGames)
            {
                if (years.ContainsKey(game.year))
                {
                    years[game.year] = years[game.year] + 1;
                }
                else
                {
                    years.Add(game.year, 1);
                }
            }

            return years;
        }

        public Question GameFromPlatform(List<GameFilter> filters, bool inverseQuestion = false, List<string> answersToExlude = null)
        {
            List<GameC> filteredGamesForAnswer, filteredGamesForOtherOptions;
            GetFilteredListsOfGames(filters, out filteredGamesForAnswer, out filteredGamesForOtherOptions);

            Dictionary<string, int> platforms = GetPlatformsFromGames(filteredGamesForAnswer);

            string plat = "";
            string correctAnswer = "";
            List<string> otherOptions = new List<string>();
            string statement = "";

            if (inverseQuestion)
            {
                List<string> validPlats = platforms.Where(y => y.Value >= 3).Select(x => x.Key).ToList();
                if (validPlats == null || validPlats.Count == 0)
                {
                    Debug.Log("Not enough games to do an inverse platform question.");
                    plat = platforms.Keys.ToList()[Random.Range(0, platforms.Keys.Count)];

                    rnd = new System.Random();

                    if (answersToExlude != null)
                    {
                        if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                        {
                            Debug.LogError("Cannot exclude answer on game from plat, getting duplicated question");
                        }
                        else
                        {
                            filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                        }
                    }

                    correctAnswer = filteredGamesForAnswer.Where(g => g.plats.Contains(plat)).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                    otherOptions = filteredGamesForOtherOptions.Where(g => !g.plats.Contains(plat)).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();

                    statement = Translations.instance.GetText("s_plat_1");
                }
                else
                {
                    rnd = new System.Random();

                    if (answersToExlude != null)
                    {
                        if (filteredGamesForOtherOptions.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                        {
                            Debug.LogError("Cannot exclude answer on game from platform inverse, getting duplicated question");
                        }
                        else
                        {
                            filteredGamesForOtherOptions = filteredGamesForOtherOptions.Where(g => !answersToExlude.Contains(g.name)).ToList();
                        }
                    }

                    plat = validPlats[Random.Range(0, validPlats.Count)];

                    correctAnswer = filteredGamesForOtherOptions.Where(g => !g.plats.Contains(plat)).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                    otherOptions = filteredGamesForAnswer.Where(g => g.plats.Contains(plat)).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();

                    statement = Translations.instance.GetText("s_plat_not");
                }
            }
            else
            {
                plat = platforms.Keys.ToList()[Random.Range(0, platforms.Keys.Count)];

                rnd = new System.Random();

                if (answersToExlude != null)
                {
                    if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                    {
                        Debug.LogError("Cannot exclude answer on game from plat, getting duplicated question");
                    }
                    else
                    {
                        filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                    }
                }

                correctAnswer = filteredGamesForAnswer.Where(g => g.plats.Contains(plat)).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                otherOptions = filteredGamesForOtherOptions.Where(g => !g.plats.Contains(plat)).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();

                statement = Translations.instance.GetText("s_plat_1");
            }


            return new Question("", $"{statement} {plat}", correctAnswer, otherOptions);
        }

        private Dictionary<string, int> GetPlatformsFromGames(List<GameC> filteredGames)
        {
            Dictionary<string, int> platforms = new Dictionary<string, int>();

            foreach (var game in filteredGames)
            {
                foreach (var plat in game.plats)
                {
                    if (platforms.ContainsKey(plat))
                    {
                        platforms[plat] = platforms[plat] + 1;
                    }
                    else
                    {
                        platforms.Add(plat, 1);
                    }
                }
            }

            return platforms;
        }

        public Question GameFromCompany(List<GameFilter> filters, bool inverseQuestion = false, List<string> answersToExlude = null)
        {
            List<GameC> filteredGamesForAnswer, filteredGamesForOtherOptions;
            GetFilteredListsOfGames(filters, out filteredGamesForAnswer, out filteredGamesForOtherOptions);

            Dictionary<string, int> companies = GetCompaniesFromGames(filteredGamesForAnswer);

            string comp = "";
            string correctAnswer = "";
            List<string> otherOptions = new List<string>();
            string statement = "";

            if (inverseQuestion)
            {
                List<string> validPlats = companies.Where(y => y.Value >= 3).Select(x => x.Key).ToList();
                if (validPlats == null || validPlats.Count == 0)
                {
                    Debug.Log("Not enough games to do an inverse platform question.");
                    comp = companies.Keys.ToList()[Random.Range(0, companies.Keys.Count)];

                    rnd = new System.Random();

                    if (answersToExlude != null)
                    {
                        if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                        {
                            Debug.LogError("Cannot exclude answer on game from comp, getting duplicated question");
                        }
                        else
                        {
                            filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                        }
                    }

                    correctAnswer = filteredGamesForAnswer.Where(g => g.devs.Contains(comp)).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                    otherOptions = filteredGamesForOtherOptions.Where(g => !g.devs.Contains(comp)).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();

                    statement = Translations.instance.GetText("s_developedBy_0");
                }
                else
                {
                    rnd = new System.Random();
                    comp = validPlats[Random.Range(0, validPlats.Count)];

                    if (answersToExlude != null)
                    {
                        if (filteredGamesForOtherOptions.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                        {
                            Debug.LogError("Cannot exclude answer on game from comp, getting duplicated question");
                        }
                        else
                        {
                            filteredGamesForOtherOptions = filteredGamesForOtherOptions.Where(g => !answersToExlude.Contains(g.name)).ToList();
                        }
                    }

                    correctAnswer = filteredGamesForOtherOptions.Where(g => !g.devs.Contains(comp)).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                    otherOptions = filteredGamesForAnswer.Where(g => g.devs.Contains(comp)).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();

                    statement = Translations.instance.GetText("s_developedBy_not");
                }
            }
            else
            {
                comp = companies.Keys.ToList()[Random.Range(0, companies.Keys.Count)];

                rnd = new System.Random();

                if (answersToExlude != null)
                {
                    if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                    {
                        Debug.LogError("Cannot exclude answer on game from comp, getting duplicated question");
                    }
                    else
                    {
                        filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                    }
                }

                correctAnswer = filteredGamesForAnswer.Where(g => g.devs.Contains(comp)).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;
                otherOptions = filteredGamesForOtherOptions.Where(g => !g.devs.Contains(comp)).OrderBy(x => rnd.Next()).Take(3).Select(g => g.name).ToList();

                statement = Translations.instance.GetText("s_developedBy_0");
            }

            return new Question("", $"{statement} {comp}", correctAnswer, otherOptions);
        }

        private Dictionary<string, int> GetCompaniesFromGames(List<GameC> filteredGames)
        {
            Dictionary<string, int> companies = new Dictionary<string, int>();

            foreach (var game in filteredGames)
            {
                foreach (var dev in game.devs)
                {
                    if (companies.ContainsKey(dev))
                    {
                        companies[dev] = companies[dev] + 1;
                    }
                    else
                    {
                        companies.Add(dev, 1);
                    }
                }
            }

            return companies;
        }

        private Question ProtagonistFromGame(List<GameFilter> filters, bool inverseQuestion = false, List<string> answersToExlude = null)
        {
            List<GameC> filteredGamesForAnswer, filteredGamesForOtherOptions;
            GetFilteredListsOfGames(filters, out filteredGamesForAnswer, out filteredGamesForOtherOptions);

            filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !g.protagonists.Contains("-")).ToList();
            filteredGamesForOtherOptions = filteredGamesForOtherOptions.Where(g => !g.protagonists.Contains("-")).ToList();

            string correctAnswer = "";
            string game = "";
            List<string> otherOptions = new List<string>();
            rnd = new System.Random();

            if (!inverseQuestion)
            {
                if (answersToExlude != null)
                {

                    if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                    {
                        Debug.LogError("Cannot exclude answer on prot from game, getting duplicated question");
                    }
                    else
                    {
                        filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                    }
                }

                game = filteredGamesForAnswer.OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;

                string[] protagonists = filteredGamesForAnswer.FirstOrDefault(g => g.name == game).protagonists;

                correctAnswer = protagonists[Random.Range(0, protagonists.Length)];

                var otherGames = filteredGamesForAnswer.Where(g => g.name != game).OrderBy(x => rnd.Next()).Take(3).ToArray();

                foreach (var otherGame in otherGames)
                {
                    var otherProts = filteredGamesForAnswer.FirstOrDefault(g => g.name == otherGame.name).protagonists;
                    otherOptions.Add(otherProts[Random.Range(0, otherProts.Length)]);
                }

            }
            else
            {
                if (answersToExlude != null)
                {
                    if (filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList().Count == 0)
                    {
                        Debug.LogError("Cannot exclude answer on prot from game, getting duplicated question");
                    }
                    else
                    {
                        filteredGamesForAnswer = filteredGamesForAnswer.Where(g => !answersToExlude.Contains(g.name)).ToList();
                    }
                }

                game = filteredGamesForAnswer.Where(g => g.protagonists.Length >= 3).OrderBy(x => rnd.Next()).Take(1).ToArray()[0].name;

                otherOptions = filteredGamesForAnswer.FirstOrDefault(g => g.name == game).protagonists.OrderBy(y => rnd.Next()).Take(3).ToList();

                bool otherOptionsHaveSameLength = otherOptions[0].Split(' ').Length == otherOptions[1].Split(' ').Length;
                int length = otherOptionsHaveSameLength? otherOptions[2].Split(' ').Length : -1;

                

                correctAnswer = filteredGamesForAnswer.Where(g => g.name != game && (length > 0 ? g.protagonists.All(p => p.Split(' ').Length == length) : true)).OrderBy(x => rnd.Next()).Take(1).Select(x => x.protagonists[Random.Range(0,x.protagonists.Length)]).ToArray()[0];

            }

            string statement = Translations.instance.GetText(inverseQuestion? "s_notProtagonist_0" : "s_protagonist_0" );

            return new Question("", $"{statement} {game}", correctAnswer, otherOptions, comparer: game);
        }

    }
}
