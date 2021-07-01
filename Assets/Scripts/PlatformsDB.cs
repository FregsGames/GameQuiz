using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsDB : MonoBehaviour
{
    public Dictionary<int, Platform> allPlatforms = new Dictionary<int, Platform>();

    public void AddPlatform(Platform platform)
    {
        if (!allPlatforms.ContainsKey(platform.id))
        {
            allPlatforms.Add(platform.id, platform);
        }
    }

    public void AddGame(Game game)
    {
        foreach (var plat in game.platforms)
        {
            if (allPlatforms.ContainsKey(plat))
            {
                allPlatforms[plat].games.Add(game);
            }
        }
    }


}
