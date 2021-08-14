using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "JsonProcessor", menuName = "ScriptableObjects/Json Processor")]
public class JsonProcessor : ScriptableObject
{
    [SerializeField]
    private PlatformsContainer platformsContainer;

    public enum JsonType {Platforms = 0, Games = 1, Companies = 2, InvolvedCompanies = 3};

    [SerializeField]
    private TextAsset json;
    [SerializeField]
    private JsonType jsonType;
    [SerializeField]
    private string fileName;
    [SerializeField]
    private bool updateGameContainer;
    [SerializeField]
    private bool createSingleContainer;

    public void Process()
    {
        switch (jsonType)
        {
            case JsonType.Platforms:
                EditorUtility.SetDirty(platformsContainer);
                ReadPlatforms();
                AssetDatabase.SaveAssets();
                break;
            case JsonType.Games:
                // Probably, give the option to create a new file with just these games, add games to GLOBAL CONTANER, or both
                break;
            case JsonType.Companies:
                ReadPlatforms();
                break;
            case JsonType.InvolvedCompanies:
                // Same as platforms
                break;
            default:
                break;
        }
    }

    private void ReadPlatforms()
    {
        Platforms platforms = JsonConvert.DeserializeObject<Platforms>(json.text);

        foreach (Platform platform in platforms.platforms)
        {
            platformsContainer.AddPlatform(platform);
        }
    }
}
