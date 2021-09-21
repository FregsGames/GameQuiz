using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cups;
using static Levels;

[CreateAssetMenu(fileName = "Cup", menuName = "ScriptableObjects/Cup")]
public class CupScriptable : ScriptableObject
{
    public string id;
    public string title;
    public List<LevelScriptable> levels = new List<LevelScriptable>();
    public Sprite cupImage;
    public CupState state;
    public GamesContainer gamesContainer;

    public void AddLevel()
    {
        levels.Add(new LevelScriptable());
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


