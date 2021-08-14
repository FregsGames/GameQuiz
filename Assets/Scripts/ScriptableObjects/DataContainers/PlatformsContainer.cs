using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformsContainer", menuName = "ScriptableObjects/Platforms")]
public class PlatformsContainer : ScriptableObject
{
    [SerializeField]
    public List<Platform> allPlatforms = new List<Platform>();

    public void AddPlatform(Platform platform)
    {
        if (!allPlatforms.Contains(platform))
        {
            allPlatforms.Add(platform);
        }
    }

    public void AddGame(Game game)
    {
        foreach (var plat in game.platforms)
        {
            Platform platform = allPlatforms.FirstOrDefault(p => p.id == plat);
            if (platform != null)
            {
                platform.games.Add(game);
            }
        }
    }
}
