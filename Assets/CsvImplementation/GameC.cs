using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameC
{
    public string name;
    public int year;
    public string[] devs;
    public string[] plats;

    public GameC(string name, int year, string[] devs, string[] plats)
    {
        this.name = name;
        this.year = year;
        this.devs = devs;
        this.plats = plats;
    }
}
