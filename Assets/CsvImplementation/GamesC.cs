using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamesC", menuName = "ScriptableObjects/GamesC")]
[InlineEditor]
public class GamesC : SerializedScriptableObject
{
    public List<GameC> gamesC;
}
