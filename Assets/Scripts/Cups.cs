using Assets.Scripts.Payloads;
using SuperMaxim.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Levels;

public class Cups : Singleton<Cups>
{
    [SerializeField]
    private List<CupScriptable> cups = new List<CupScriptable>();
    public enum CupState { locked = 0, completed = 2, unlocked = 1 };

    private void Start()
    {
        foreach (var cup in cups)
        {
            cup.state = (CupState)SaveManager.instance.RetrieveInt(cup.id);

            foreach (var level in cup.levels)
            {
                level.state = (LevelState) SaveManager.instance.RetrieveInt(level.id);
                if(level.alwaysUnlocked && level.state == LevelState.locked)
                {
                    level.state = LevelState.unlocked;
                }
            }
        }
    }

    public bool CheckUnlocks(Level level)
    {
        SaveManager.instance.Save(level.id, (int)LevelState.completed);

        level.state = LevelState.completed;

        CupScriptable currentCup = cups.FirstOrDefault(c => c.levels.FirstOrDefault(l => l.id == level.id) != null);

        int index = currentCup.levels.IndexOf(currentCup.levels.FirstOrDefault(l => l.id == level.id));

        if(currentCup.levels.Count - 1 > index)
        {
            SaveManager.instance.Save(currentCup.levels[index + 1].id, (int)LevelState.unlocked);
            currentCup.levels[index + 1].state = LevelState.unlocked;

            return true;
        }

        return false;
    }

    public List<CupScriptable> GetAllCups()
    {
        return cups;
    }

    public CupScriptable GetCup(string id)
    {
        return cups.FirstOrDefault(c => c.id == id);
    }


}
