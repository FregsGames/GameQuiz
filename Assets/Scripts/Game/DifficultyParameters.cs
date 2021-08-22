using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyParameters : Singleton<DifficultyParameters>
{
    private Dictionary<int, Vector2Int> years = new Dictionary<int, Vector2Int>()
    {
        {1, new Vector2Int(1985, 2020) }, // check
        {2, new Vector2Int(2000, 2020) },
        { 3, new Vector2Int(1985, 2020)}
    };
    private Dictionary<int, int[]> platforms = new Dictionary<int, int[]>()
    { 
        { 1, new int[] { 165, 163, 130, 37, 9, 48, 167, 49 } },
        { 2, new int[] { 165, 163, 130, 37, 9, 48, 49, 167, 8, 39, 6, 23, 132, 159, 11, 47, 24, 4, 22, 41, 37, 38, 19, 20, 21, 34, 162, 46, 12, 7, 33, 170  } },
        { 3, new int[] { } }
    };

    public int GetMinimumGameForCompany(int difficulty)
    {
        return 20 - (difficulty * 5);
    }

    public int[] GetPlatforms(int difficulty)
    {
        if (platforms.ContainsKey(difficulty))
        {
            return platforms[3]; //check
        }
        else
        {
            return platforms[1];
        }
    }

    public Vector2Int GetYearRange(int difficulty)
    {
        if (years.ContainsKey(difficulty)){
            return years[difficulty];
        }
        else
        {
            return years[1];
        }
    }
}
