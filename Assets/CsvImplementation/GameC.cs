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
    public string pack;
    public string[] protagonists;

    public GameC(string name, int year, string[] devs, string[] plats, string pack, string[] protagonists)
    {
        this.name = name;
        this.year = year;
        this.devs = devs;
        this.plats = plats;
        this.pack = pack;
        this.protagonists = protagonists;
    }
}
