using Assets.Scripts.Payloads;
using Questions;
using SuperMaxim.Messaging;
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
    public enum LevelCondition { half = 0, full = 1, one = 2};

    private void Start()
    {
        foreach (var level in levels)
        {
            level.level.state = (LevelState) SaveManager.instance.RetrieveInt(level.levelID);
            level.level.id = level.levelID;

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

    public void SetUnlocked(string id)
    {
        SaveManager.instance.Save(id, (int)LevelState.unlocked);

        GetLevel(id).state = LevelState.unlocked;
    }

    public void SetCompleted(string id)
    {
        SaveManager.instance.Save(id, (int)LevelState.completed);

        GetLevel(id).state = LevelState.completed;
    }

    [Serializable]
    public class Level
    {
        public string id;
        public string levelTitle;
        public string levelDesc;
        public HandwrittenQuestionSet handwrittenQuestionSet;
        public List<QuestionTemplate> questionTemplates;
        public LevelState state;
        public bool alwaysUnlocked = false;
        public LevelCondition winCondition;

    }
    [Serializable]
    public class LevelEntry
    {
        public string levelID;
        public Level level;
    }
}
