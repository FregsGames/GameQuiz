using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CSVProcessor : EditorWindow
{
    private GamesC gamesCScriptable;
    private TextAsset csv;

    [MenuItem("Window/CSVProcessor")]
    static void Init()
    {
        CSVProcessor window = (CSVProcessor)GetWindow(typeof(CSVProcessor));
        window.Show();
    }

    void OnGUI()
    {
        gamesCScriptable = (GamesC)EditorGUILayout.ObjectField(gamesCScriptable, typeof(GamesC), false);

        GUILayout.Label("CSV Processor", EditorStyles.boldLabel);
        GUILayout.Label("CSV File");
        csv = (TextAsset)EditorGUILayout.ObjectField(csv, typeof(TextAsset), false);

        if (GUILayout.Button("Process"))
        {
            Process();
        }
    }

    private void Process()
    {
        string rawCsv = csv.text;

        EditorUtility.SetDirty(csv);

        foreach (var line in rawCsv.Split('\n'))
        {
            GameC game = ExtractGame(line);

            var existingGame = gamesCScriptable.gamesC.FirstOrDefault(g => g.name == game.name);

            if (existingGame == null)
            {
                gamesCScriptable.gamesC.Add(game);
            }
            else
            {
                existingGame.name = game.name;
                existingGame.year = game.year;
                existingGame.devs = game.devs;
                existingGame.plats = game.plats;
                existingGame.pack = game.pack;
            }
        }
    }

    private static GameC ExtractGame(string line)
    {
        string[] gameInfo = line.Split(',');

        string name = gameInfo[0];
        int year = int.Parse(gameInfo[1]);
        string[] devs = gameInfo[2].Split('$');

        for (int i = 0; i < devs.Length; i++)
        {
            devs[i] = devs[i].Trim('\r', ' ');
        }

        string[] plats = gameInfo[3].Split('/');

        for (int i = 0; i < plats.Length; i++)
        {
            plats[i] = plats[i].Trim('\r', ' ');
        }

        string pack = gameInfo[4].Trim('\r', ' ');

        ReplaceDevNames(devs);
        ReplacePlats(devs, ref plats);

        GameC game = new GameC(name, year, devs, plats, pack);
        return game;
    }

    private static void ReplacePlats(string[] devs, ref string[] plats)
    {

        for (int i = 0; i < plats.Length; i++)
        {
            if (plats[i].Contains("PC"))
                plats[i] = "PC";
        }

        if (plats.Contains("Xbox One") && !plats.Contains("Xbox Series X|S"))
        {
            List<string> newPlats = new List<string>(plats);
            newPlats.Add("Xbox Series X|S");
            plats = newPlats.ToArray();
        }

        if (plats.Contains("PlayStation 4") && !plats.Contains("PlayStation 5"))
        {
            List<string> newPlats = new List<string>(plats);
            newPlats.Add("PlayStation 5");
            plats = newPlats.ToArray();
        }
    }

    private static void ReplaceDevNames(string[] devs)
    {
        for (int i = 0; i < devs.Length; i++)
        {
            if (devs[i].Contains("Nintendo"))
                devs[i] = "Nintendo";
        }

        for (int i = 0; i < devs.Length; i++)
        {
            if (devs[i].Contains("Ubisfot"))
                devs[i] = "Ubisfot";
        }

        for (int i = 0; i < devs.Length; i++)
        {
            if (devs[i].Contains("Sega"))
                devs[i] = "Sega";
        }

        for (int i = 0; i < devs.Length; i++)
        {
            if (devs[i].Contains("Sega"))
                devs[i] = "Sega";
        }

        for (int i = 0; i < devs.Length; i++)
        {
            if (devs[i].Contains("EA"))
                devs[i] = "Electronic Arts";
        }

        for (int i = 0; i < devs.Length; i++)
        {
            if (devs[i].Contains("Electronic Arts"))
                devs[i] = "Electronic Arts";
        }

        for (int i = 0; i < devs.Length; i++)
        {
            if (devs[i].Contains("Konami"))
                devs[i] = "Konami";
        }
    }
}
