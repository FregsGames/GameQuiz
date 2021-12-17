using Questions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Levels;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "LevelC", menuName = "ScriptableObjects/LevelC")]
public class LevelScriptableC : SerializedScriptableObject
{
    public string id;
    public string levelTitle;

    public HandwrittenQuestionSet handwrittenQuestionSet;
    public List<QuestionTemplate> questionTemplates;

    public LevelState state;
    public bool alwaysUnlocked = false;
    public LevelCondition winCondition;

    public List<GameFilter> filters = new List<GameFilter>();
}

[Serializable]
public abstract class GameFilter
{
    public virtual List<GameC> Filter(List<GameC> raw) {return raw; }

    public bool otherOptionsUseFilter;
}

[Serializable]
public class YearFilter : GameFilter
{
    [SerializeField]
    private int from;

    [SerializeField]
    private int to;

    public override List<GameC> Filter(List<GameC> raw)
    {
        return raw.Where(g => g.year >= from && g.year <= to).ToList();
    }
}

[Serializable]
public class PlatformFilter : GameFilter
{
    [SerializeField]
    private List<string> platforms;

    public override List<GameC> Filter(List<GameC> raw)
    {
        return raw.Where(g => g.plats.Any(p => platforms.Contains(p))).ToList();
    }
}

public class PackFilter : GameFilter
{
    public override List<GameC> Filter(List<GameC> raw)
    {
        if (IAPManager.Instance.IsInit)
        {
            return raw.Where(g => g.pack == "free" || IAPManager.Instance.BoughtPacks.Contains(g.pack)).ToList();
        }
        else
        {
            return raw.Where(g => g.pack == "free").ToList();
        }

    }
}

