using Questions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Levels;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class LevelScriptable : SerializedScriptableObject
{
    public string id;
    public string levelTitle;
    [OnValueChanged("UpdateInfo")]
    public GamesContainer gamesContainer;
    public CompaniesContainer companiesContainer;
    public HandwrittenQuestionSet handwrittenQuestionSet;
    public List<QuestionTemplate> questionTemplates;
    public LevelState state;
    public bool alwaysUnlocked = false;
    public LevelCondition winCondition;

    public List<int> years;
    public List<(int,int)> platforms;
    public List<(int,int)> involved;
    public List<(Company,int)> companies;


    public void AddQuestion()
    {
        questionTemplates.Add(new QuestionTemplate());
    }

    public void RemoveQuestion(int i)
    {
        questionTemplates.RemoveAt(i);
    }

    private void UpdateInfo()
    {
        UpdatePlatforms();
        UpdateCompanies();
        ChangeGamesContainer();
    }

    private void UpdatePlatforms()
    {
        if(platforms != null)
        {
            platforms.Clear();
        }
        else
        {
            platforms = new List<(int,int)>();
        }

        if(gamesContainer != null)
        {
            foreach (var plat in gamesContainer.Platforms())
            {
                platforms.Add((plat.Key,plat.Value));
            }
            platforms = platforms.OrderByDescending(i => i.Item2).ToList<(int,int)>();
        }
    }
    private void UpdateCompanies()
    {
        if (companiesContainer == null)
            return;

        if (companies != null)
        {
            involved.Clear();
        }
        else
        {
            involved = new List<(int, int)>();
        }

        if (gamesContainer != null)
        {
            foreach (var comp in gamesContainer.Involved())
            {
                involved.Add((comp.Key, comp.Value));
            }
        }

        companies = companiesContainer.GetCompanies(involved);
        companies = companies.OrderByDescending(i => i.Item2).ToList<(Company, int)>();

    }

    private void ChangeGamesContainer()
    {
        if(years != null)
        {
            years.Clear();
        }
        else
        {
            years = new List<int>();
        }

        if(gamesContainer != null)
        {
            foreach (var year in gamesContainer.Years())
            {
                years.Add(year.Key);
            }
        }
    }
}
