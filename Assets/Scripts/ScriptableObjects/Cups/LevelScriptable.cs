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
    public List<PlatformTuple> platforms;
    public List<InvolvedTuple> involved;
    public List<CompanyTuple> companies;

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
            platforms = new List<PlatformTuple>();
        }

        if(gamesContainer != null)
        {
            foreach (var plat in gamesContainer.Platforms())
            {
                platforms.Add(new PlatformTuple(plat.Key, plat.Value));
            }
            platforms = platforms.OrderByDescending(i => i.counter).ToList();
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
            involved = new List<InvolvedTuple>();
        }

        if (gamesContainer != null)
        {
            foreach (var comp in gamesContainer.Involved())
            {
                involved.Add(new InvolvedTuple(comp.Key, comp.Value));
            }
        }

        companies = companiesContainer.GetCompanies(involved);
        companies = companies.OrderByDescending(i => i.counter).ToList();

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

[Serializable]
public class CompanyTuple
{
    public Company company;
    public int counter;

    public CompanyTuple(Company c, int co)
    {
        company = c;
        counter = co;
    }
}


[Serializable]
public class PlatformTuple
{
    public int platform;
    public int counter;

    public PlatformTuple(int p, int c)
    {
        platform = p;
        counter = c;
    }
}

[Serializable]
public class InvolvedTuple
{
    public int involved;
    public int counter;

    public InvolvedTuple(int i, int c)
    {
        involved = i;
        counter = c;
    }
}