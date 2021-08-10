using Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Handwritten Question Set", menuName = "ScriptableObjects/HandwrittenQuestionSet")]
public class HandwrittenQuestionSet : ScriptableObject
{
    public string setId;
    public List<HandwrittenQuestion> questions = new List<HandwrittenQuestion>();
    public List<SystemLanguage> langs = new List<SystemLanguage>();

    private List<string> alreadyCorrectlyAnsweredQuestions = new List<string>();

    public void Initialize()
    {
        foreach (var question in questions)
        {
            if (SaveManager.instance.RetrieveInt(question.questionId, 0) == 1)
            {
                alreadyCorrectlyAnsweredQuestions.Add(question.questionId);
            }
        }

        if (alreadyCorrectlyAnsweredQuestions.Count == questions.Count)
        {
            ResetAnsweredQuestions();
        }
    }

    private void ResetAnsweredQuestions()
    {
        foreach (var question in questions)
        {
            SaveManager.instance.Save(question.questionId, 0);
        }
        alreadyCorrectlyAnsweredQuestions.Clear();
    }

    public Question GetQuestion(List<string> toExclude)
    {
        List<HandwrittenQuestion> unasweredQuestions = questions.Where(q => !alreadyCorrectlyAnsweredQuestions.Contains(q.questionId)
        && !toExclude.Contains(q.questionId)).ToList();

        if (unasweredQuestions.Count == 0)
        {
            ResetAnsweredQuestions();
            return GetQuestion(toExclude);
        }

        HandwrittenQuestion chosenQuestion = unasweredQuestions[Random.Range(0, unasweredQuestions.Count)];

        string questionId = chosenQuestion.questionId;

        int langIndex = GetCurrentLanguageIndex();

        List<string> wrongOptions = chosenQuestion.wrongAnswers.OrderBy(x => Random.value).Take(3).ToList();

        Question question = new Question(questionId, chosenQuestion.entries[langIndex].statement, chosenQuestion.correctAnswer, wrongOptions, true);

        return question;

    }

    private int GetCurrentLanguageIndex()
    {
        int langIndex = 0;
        if (langs.Contains(Translations.instance.currentLanguage))
        {
            langIndex = langs.IndexOf(Translations.instance.currentLanguage);
        }

        return langIndex;
    }
}

[Serializable]
public class HandwrittenQuestion
{
    public string questionId;
    public List<HandwrittenQuestionEntry> entries;
    public string correctAnswer;
    public List<string> wrongAnswers;

}
[Serializable]
public class HandwrittenQuestionEntry
{
    public string statement;
}
