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
            GameC game = ExtractGAme(line);

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
            }
        }
    }

    private static GameC ExtractGAme(string line)
    {
        string[] gameInfo = line.Split(',');

        string name = gameInfo[0];
        int year = int.Parse(gameInfo[1]);
        string[] devs = gameInfo[2].Split('/');
        string[] plats = gameInfo[3].Split('/');
        ReplaceDevNames(devs);
        ReplacePlats(devs, plats);

        GameC game = new GameC(name, year, devs, plats);
        return game;
    }

    private static void ReplacePlats(string[] devs, string[] plats)
    {
        for (int i = 0; i < plats.Length; i++)
        {
            if (plats[i].Contains("PC"))
                plats[i] = "PC";
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
    }
}
