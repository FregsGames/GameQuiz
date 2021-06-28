using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    private TextAsset jsonFile;

    public GamesDB gamesDB;

    private void Start()
    {
        ReadAllFiles();
    }

    public void ReadAllFiles()
    {
        TextAsset[] jsons = Resources.LoadAll<TextAsset>("AllJsons/");

        foreach (var json in jsons)
        {
            ReadFile(json);
        }
    }

    public void ReadFile(TextAsset jsonFile)
    {
        Games games = JsonConvert.DeserializeObject<Games>(jsonFile.text);

        foreach (Game game in games.games)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(game.first_release_date).ToLocalTime();
            game.realDate = dtDateTime;

            gamesDB.AddGame(game);
        }
    }
}
