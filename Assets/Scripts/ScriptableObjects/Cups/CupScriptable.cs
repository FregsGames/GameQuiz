﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static Cups;
using static Levels;

[CreateAssetMenu(fileName = "Cup", menuName = "ScriptableObjects/Cup")]
public class CupScriptable : ScriptableObject
{
    public string id;
    public string title;
    public List<Level> levels = new List<Level>();
    public Sprite cupImage;
    public CupState state;
    public GamesContainer gamesContainer;

    public void AddLevel()
    {
        levels.Add(new Level());
    }

    public void RemoveLevel(int index)
    {
        levels.RemoveAt(index);
    }

}


