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


    }
    [Serializable]
    public class LevelEntry
    {
        public string levelID;
        public Level level;
    }
}
