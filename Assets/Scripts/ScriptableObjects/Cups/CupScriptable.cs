using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cups;
using static Levels;

[CreateAssetMenu(fileName = "Cup", menuName = "ScriptableObjects/Cup")]
public class CupScriptable : SerializedScriptableObject
{
    public string id;
    public string title;
    public string packTitle;
    public string desc;
    public string packDesc;
    public List<LevelScriptableC> levels = new List<LevelScriptableC>();
    public List<GameFilter> infiniteFilters = new List<GameFilter>();
    public Sprite cupImage;
    public CupType state;

    public void AddLevel()
    {
        levels.Add(new LevelScriptableC());
    }

    public void RemoveLevel(int index)
    {
        levels.RemoveAt(index);
    }

    public int GetCompletedLevelsCount()
    {
        int count = levels.Count(l => l.state == LevelState.completed);
        return count;
    }

}


