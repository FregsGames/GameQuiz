using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cup
{
    public string title;
    public List<Level> levels;
    public Sprite cupImage;

}
[Serializable]
public class Level
{
    public string levelTitle;
}