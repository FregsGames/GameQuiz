using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class JsonProcessorEditor : EditorWindow
{
    public enum JsonType { Platforms = 0, Games = 1, Companies = 2, InvolvedCompanies = 3 };

    PlatformsContainer platformsContainer;
    TextAsset json;
    JsonType jsonType;

    [MenuItem("Window/Json Processor")]
    static void Init()
    {
        JsonProcessorEditor window = (JsonProcessorEditor)GetWindow(typeof(JsonProcessorEditor));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("JSON Processor", EditorStyles.boldLabel);
        GUILayout.Label("JsonFile");
        json = (TextAsset)EditorGUILayout.ObjectField(json, typeof(TextAsset), false);
        jsonType = (JsonType)EditorGUILayout.EnumPopup("Json type", jsonType);

        if(jsonType == JsonType.Platforms)
        {
            platformsContainer = (PlatformsContainer)EditorGUILayout.ObjectField(platformsContainer, typeof(PlatformsContainer), false);
        }

        if (GUILayout.Button("Process"))
        {
            Process();
        }
    }

    private void Process()
    {
        switch (jsonType)
        {
            case JsonType.Platforms:
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(platformsContainer);
                ReadPlatforms();
                AssetDatabase.SaveAssets();
                break;
            case JsonType.Games:

                break;
            case JsonType.Companies:
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
