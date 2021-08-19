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

    public void ClearAllGames()
    {
        foreach (var plat in allPlatforms)
        {
            plat.games.Clear();
        }
    }

    public void AddGame(Game game)
    {
        if (game.platforms == null)
            return;

        foreach (var plat in game.platforms)
        {
            Platform platform = allPlatforms.FirstOrDefault(p => p.id == plat);
            if (platform != null)
            {
                platform.games.Add(game);
            }
        }
    }

    public void OnGameDeleted(int id)
    {
        foreach (var plat in allPlatforms.Where(g => g.games.FirstOrDefault(gm => gm.id == id) != null))
        {
            Game game = plat.games.FirstOrDefault(gm => gm.id == id);
            plat.games.Remove(game);
        }
    }
}
