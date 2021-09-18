using Questions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Levels;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class LevelScriptable : SerializedScriptableObject
{
    public string id;
    public string levelTitle;
    public GamesContainer gamesContainer;
    public HandwrittenQuestionSet handwrittenQuestionSet;
    public List<QuestionTemplate> questionTemplates;
    public LevelState state;
    public bool alwaysUnlocked = false;
    public LevelCondition winCondition;

    public void AddQuestion()
    {
        questionTemplates.Add(new QuestionTemplate());
    }

    public void RemoveQuestion(int i)
    {
        questionTemplates.RemoveAt(i);
    }
}
