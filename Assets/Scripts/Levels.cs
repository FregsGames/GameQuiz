using Questions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levels : Singleton<Levels>
{
    [SerializeField]
    private List<LevelEntry> levels = new List<LevelEntry>();

    public enum LevelState { locked = 0, completed = 2, unlocked = 1};

    private void Start()
    {
        foreach (var level in levels)
        {
            level.level.state = (LevelState) SaveManager.instance.RetrieveInt(level.levelID);
            if (level.level.alwaysUnlocked)
            {
                if(level.level.state == LevelState.locked)
                {
                    level.level.state = LevelState.unlocked;
                }
            }
        }
    }

    public Level GetLevel(string id)
    {
        return levels.FirstOrDefault(l => l.levelID == id).level;
    }

    [Serializable]
    public class Level
    {
        public string levelTitle;
        public string levelDesc;
        public List<QuestionTemplate> questionTemplates;
        public LevelState state;
        public bool alwaysUnlocked = false;

    }
    [Serializable]
    public class LevelEntry
    {
        public string levelID;
        public Level level;
    }
}
