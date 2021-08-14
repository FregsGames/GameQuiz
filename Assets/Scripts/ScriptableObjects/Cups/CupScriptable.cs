using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cups;

[CreateAssetMenu(fileName = "Cup", menuName = "ScriptableObjects/Cup")]
public class CupScriptable : ScriptableObject
{
    [NonSerialized]
    public string id;
    public string title;
    public List<string> levels;
    public Sprite cupImage;
    public CupState state;
    public TextAsset json;

}
