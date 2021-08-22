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
    private List<CupEntry> cups = new List<CupEntry>();
    public enum CupState { locked = 0, completed = 2, unlocked = 1 };

    private void Start()
    {
        foreach (var cup in cups)
        {
            cup.cup.state = (CupState)SaveManager.instance.RetrieveInt(cup.cupID);
            cup.cup.id = cup.cupID;
        }
    }

    public bool CheckUnlocks(string levelId)
    {
        Levels.Instance.SetCompleted(levelId);

        Cup currentCup = cups.FirstOrDefault(c => c.cup.levels.Contains(levelId)).cup;

        int index = currentCup.levels.IndexOf(levelId);

        if(currentCup.levels.Count - 1 > index)
        {
            Levels.Instance.SetUnlocked(currentCup.levels[index + 1]);
            return true;
        }

        return false;
    }

    public List<Cup> GetAllCups()
    {
        return cups.Select(c => c.cup).ToList();
    }

    public Cup GetCup(string id)
    {
        return cups.FirstOrDefault(c => c.cupID == id).cup;
    }

    [Serializable]
    public class Cup
    {
        [NonSerialized]
        public string id;
        public string title;
        public List<string> levels;
        public Sprite cupImage;
        public CupState state;
        public GamesContainer gamesContainer;
    }

    [Serializable]
    public class CupEntry
    {
        public string cupID;
        public Cup cup;
    }

}
