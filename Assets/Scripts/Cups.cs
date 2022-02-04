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
    [Serializable]
    public enum CupType { free = 0, premium = 1};

    public float percentageCompleted { get; set; }

    private void Start()
    {
        CheckCups();
    }

    public void CheckCups()
    {
        int totalLevels = 0;
        int completed = 0;

        foreach (var cup in cups)
        {
            foreach (var level in cup.levels)
            {
                totalLevels++;

                level.state = (LevelState)SaveManager.instance.RetrieveInt(level.id);
                if (level.alwaysUnlocked && level.state == LevelState.locked)
                {
                    level.state = LevelState.unlocked;
                }

                if (level.state == LevelState.completed)
                {
                    completed++;
                }
            }
        }

        percentageCompleted = (float)completed / totalLevels;
    }

    public bool CheckUnlocks(LevelScriptableC level)
    {
        SaveManager.instance.Save(level.id, (int)LevelState.completed);

        level.state = LevelState.completed;

        CupScriptable currentCup = cups.FirstOrDefault(c => c.levels.FirstOrDefault(l => l.id == level.id) != null);

        int index = currentCup.levels.IndexOf(currentCup.levels.FirstOrDefault(l => l.id == level.id));

        if(currentCup.levels.Count - 1 > index && currentCup.levels[index + 1].state == LevelState.locked)
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
