using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "Handwritten Question", menuName = "ScriptableObjects/HandwrittenQuestion")]
public class HandwrittenQuestion : SerializedScriptableObject
{
    public string questionId;

    public DictionaryLanguageListString wrongAnswers;
    public DictionaryLanguageString statement;
    public DictionaryLanguageString correctAnswer;

    public List<string> WrongAnswers
    {
        get
        {
            SystemLanguage currentLanguage = Translations.instance.currentLanguage;
            if (wrongAnswers.ContainsKey(currentLanguage))
            {
                return wrongAnswers.Get(currentLanguage);
            }
            else if (wrongAnswers.ContainsKey(SystemLanguage.English))
            {
                return wrongAnswers.Get(SystemLanguage.English);
            }
            else
            {
                return new List<string>();
            }
        }
    }

    public string CorrectAnswer
    {
        get
        {
            SystemLanguage currentLanguage = Translations.instance.currentLanguage;
            if (correctAnswer.ContainsKey(currentLanguage))
            {
                return correctAnswer.Get(currentLanguage);
            }
            else if (correctAnswer.ContainsKey(SystemLanguage.English))
            {
                return correctAnswer.Get(SystemLanguage.English);
            }
            else
            {
                return "";
            }
        }
    }
    public string Statement
    {
        get
        {
            SystemLanguage currentLanguage = Translations.instance.currentLanguage;
            if (statement.ContainsKey(currentLanguage))
            {
                return statement.Get(currentLanguage);
            }
            else if (statement.ContainsKey(SystemLanguage.English))
            {
                return statement.Get(SystemLanguage.English);
            }
            else
            {
                return "";
            }
        }
    }
}

[Serializable]
public class DictionaryLanguageString
{
    [SerializeField]
    public List<LangStringPair> langStrings;
    public List<LangStringPair> LangStrings { get => langStrings; set => langStrings = value; }

    public bool ContainsKey(SystemLanguage lang)
    {
        return LangStrings.FirstOrDefault(w => w.Lang == lang) != null;
    }

    public string Get(SystemLanguage lang)
    {
        if (!ContainsKey(lang))
        {
            return "";
        }
        else
        {
            return LangStrings.FirstOrDefault(w => w.Lang == lang).Text;
        }
    }
}

[Serializable]
public class DictionaryLanguageListString
{
    [SerializeField]
    public List<LangListStringPair> langStringsList;

    public List<LangListStringPair> LangStringsList { get => langStringsList; set => langStringsList = value; }

    public bool ContainsKey(SystemLanguage lang)
    {
        return LangStringsList.FirstOrDefault(w => w.Lang == lang) != null;
    }

    public List<string> Get(SystemLanguage lang)
    {
        if (!ContainsKey(lang))
        {
            return new List<string>();
        }
        else
        {
            return LangStringsList.FirstOrDefault(w => w.Lang == lang).Texts;
        }
    }
}

[Serializable]
public class LangStringPair
{
    [SerializeField]
    private SystemLanguage lang;
    [SerializeField]
    private string text;

    public string Text { get => text; set => text = value; }
    public SystemLanguage Lang { get => lang; set => lang = value; }
}

[Serializable]
public class LangListStringPair
{
    [SerializeField]
    private SystemLanguage lang;
    [SerializeField]
    private List<string> texts;

    public List<string> Texts { get => texts; set => texts = value; }
    public SystemLanguage Lang { get => lang; set => lang = value; }
}